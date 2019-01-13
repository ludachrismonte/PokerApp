using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingManager : MonoBehaviour {

    private PhotonView PV;

    private int current_bet;
    public Text current_bet_text;
    public int turn_id;
    public bool round_over;

    // Use this for initialization
    void Start() {
        PV = GetComponent<PhotonView>();

        turn_id = 0;
        ResetBets();
    }

    // Update is called once per frame
    void Update() {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (GameManager.game_manager.IsOnBettingStage() && GameManager.game_manager.GetNumRegistered() > 0 && (!GameManager.game_manager.AllPlayers[turn_id].IsIn() || GameManager.game_manager.AllPlayers[turn_id].HasCalled())) {
            PV.RPC("RPC_MoveToNextPlayer", RpcTarget.All, turn_id);
        }
    }

    public void StartRound(int StartID) {
        PV.RPC("RPC_StartRound", RpcTarget.All, StartID);
    }

    [PunRPC]
    public void RPC_StartRound(int StartID)
    {
        turn_id = StartID;
        GameManager.game_manager.AllPlayers[turn_id].RpcEnableBetting(true);
    }

    [PunRPC]
    private void RPC_TellMasterToMoveToNextPlayer()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PV.RPC("RPC_MoveToNextPlayer", RpcTarget.All, turn_id);
    }

    [PunRPC]
    private void RPC_MoveToNextPlayer(int MasterCurrentTurnID)
    {
        DisableAllBetting();
        turn_id = MasterCurrentTurnID + 1;
        if (turn_id == GameManager.game_manager.GetNumRegistered())
        {
            turn_id = 0;
        }
        if (!GameManager.game_manager.AllPlayers[turn_id].HasCalled()) {
            GameManager.game_manager.AllPlayers[turn_id].RpcEnableBetting(true);
        }
        Debug.Log("Betting Manager moved turn to player " + turn_id);
    }

    [PunRPC]
    private void RPC_SetCurrentBet(int amt)
    {
        current_bet = amt;
        current_bet_text.text = "Current Bet: " + current_bet.ToString();
    }

    public bool CheckIfEveryoneIsDone() {
        round_over = true;
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            if (GameManager.game_manager.AllPlayers[i].IsIn() && !GameManager.game_manager.AllPlayers[i].HasCalled()) {
                round_over = false;
            }
        }
        return round_over;
    }

    public void DisableAllBetting() {
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            GameManager.game_manager.AllPlayers[i].RpcEnableBetting(false);
        }
    }

    public void ResetBets()
    {
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            GameManager.game_manager.AllPlayers[i].ResetBets();
        }
        round_over = false;
        PV.RPC("RPC_SetCurrentBet", RpcTarget.All, 0);
    }

    [PunRPC]
    public void RPC_ResetAllExcept(int ID)
    {
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            if (GameManager.game_manager.AllPlayers[i].GetID() != ID) {
                GameManager.game_manager.AllPlayers[i].ResetBets();
            }
        }
    }

    public void ResetHand()
    {
        PotManager.pot_manager.SetPot(0);
    }

    public int GetTurnID() {
        return turn_id;
    }

    public int GetCurrentBet()
    {
        return current_bet;
    }

    //~~~~~~~~~~~~~~ACTIONS~~~~~~~~~~~~~~//

    public bool ServerCall(int ID) {
        if (ID != turn_id) {
            Debug.LogError("Cannot Call, " + ID + " it's not your turn");
            return false;
        }

        PotManager.pot_manager.AddToPot(current_bet);
        PV.RPC("RPC_TellMasterToMoveToNextPlayer", RpcTarget.MasterClient);
        return true;
    }

    public bool ServerBet(int ID, int amount)
    {
        if (ID != turn_id)
        {
            Debug.LogError("Cannot Bet, " + ID + " it's not your turn");
            return false;
        }

        if (amount < current_bet * 2)
        {
            Debug.LogError(amount.ToString() + " must bet at least twice the current bet of " + current_bet.ToString());
            return false;
        }

        PV.RPC("RPC_SetCurrentBet", RpcTarget.All, amount);
        PotManager.pot_manager.AddToPot(current_bet);
        PV.RPC("RPC_ResetAllExcept", RpcTarget.All, ID);
        PV.RPC("RPC_TellMasterToMoveToNextPlayer", RpcTarget.MasterClient);
        return true;
    }

    public bool ServerCheck(int ID)
    {
        if (ID != turn_id)
        {
            Debug.LogError("Cannot Check, " + ID + " it's not your turn");
            return false;
        }

        if (current_bet > 0)
        {
            Debug.LogError("Cannot Check when there's a bet of " + current_bet);
            return false;
        }

        PV.RPC("RPC_TellMasterToMoveToNextPlayer", RpcTarget.MasterClient);
        return true;
    }
}
