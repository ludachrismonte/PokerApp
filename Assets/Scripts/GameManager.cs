using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Deal = 2,
    FirstBet = 3,
    Flop = 4,
    SecondBet = 5,
    Turn = 6,
    ThirdBet = 7,
    River = 8,
    FourthBet = 9,
    Payout = 10,
    Reset = 11,
    BettingLimbo = 12,
    WaitingLimbo = 13
};

public class GameManager : MonoBehaviour {

    private PhotonView PV;

    public static GameManager game_manager;

    public GameObject StartButton;
    public GameState game_state;
    public Text game_state_text;

    public List<PokerPlayer> AllPlayers;
    private int dealer_id;

    private CardManager card_manager;
    private BettingManager betting_manager;
    private PayoutManager payout_manager;

    private void Awake()
    {
        if (GameManager.game_manager == null)
        {
            GameManager.game_manager = this;
        }
        else
        {
            if (GameManager.game_manager != this)
            {
                Destroy(this.gameObject);
            }
        }
        PV = GetComponent<PhotonView>();
    }

    // Use this for initialization
    void Start () {
        AllPlayers = new List<PokerPlayer>();
        card_manager = GetComponent<CardManager>();
        betting_manager = GetComponent<BettingManager>();
        payout_manager = GetComponent<PayoutManager>();
        Debug.Log("Waiting for players to start...");
        game_state_text.text = "Game State: Waiting to start...";
    }

    public void Ready()
    {
        PV.RPC("RPC_Ready", RpcTarget.All);
        PV.RPC("RPC_ToggleStartButton", RpcTarget.AllBuffered, false);
    }

    [PunRPC]
    private void RPC_ToggleStartButton(bool state)
    {
        StartButton.SetActive(state);
    }

    [PunRPC]
    private void RPC_Ready()
    {
        card_manager.ResetTable();
        SetGameState(GameState.Deal);
    }

    // Control Gameloop
    void Update () {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }
        switch (game_state)
        {   
            case GameState.Deal:
                SetGameState(GameState.WaitingLimbo);
                RunDeal();
                break;
            case GameState.FirstBet:
                SetGameState(GameState.BettingLimbo);
                StartCoroutine(RunFirstBet());
                break;
            case GameState.Flop:
                SetGameState(GameState.WaitingLimbo);
                RunFlop();
                break;
            case GameState.SecondBet:
                SetGameState(GameState.BettingLimbo);
                StartCoroutine(RunSecondBet());
                break;
            case GameState.Turn:
                SetGameState(GameState.WaitingLimbo);
                RunTurn();
                break;
            case GameState.ThirdBet:
                SetGameState(GameState.BettingLimbo);
                StartCoroutine(RunThirdBet());
                break;
            case GameState.River:
                SetGameState(GameState.WaitingLimbo);
                RunRiver();
                break;
            case GameState.FourthBet:
                SetGameState(GameState.BettingLimbo);
                StartCoroutine(RunFourthBet());
                break;
            case GameState.Payout:
                RunPayout();
                break;
            case GameState.Reset:
                RunReset();
                break;
        }
    }

    public int Register(PokerPlayer p) {
        AllPlayers.Add(p);
        PlayerTabsManager.player_tabs_manager.AssignLocations();
        return AllPlayers.Count - 1;
    }

    public int GetNumRegistered() {
        return AllPlayers.Count;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATE SETS~~~~~~~~~~~~~~~~~~~~~~//

    public void SetGameState(GameState state) {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PV.RPC("RPC_SetGameState", RpcTarget.All, state);
    }

    [PunRPC]
    private void RPC_SetGameState(GameState state)
    {
        game_state = state;
        game_state_text.text = "Game State: " + game_state.ToString();
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATE CHECKS~~~~~~~~~~~~~~~~~~~~~~//

    public bool IsOnBettingStage() {
        return game_state == GameState.BettingLimbo;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATES~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void RunDeal() {
        Debug.Log("GM: Dealing");
        card_manager.ShuffleAndDeal();
    }

    private IEnumerator RunFirstBet()
    {
        Debug.Log("RunFirstBet");
        betting_manager.ResetBets();
        betting_manager.StartRound(0);
        while (!betting_manager.CheckIfEveryoneIsDone()) {
            yield return null;
        }
        betting_manager.DisableAllBetting();
        SetGameState(GameState.Flop);
    }

    private void RunFlop()
    {
        Debug.Log("RunningFlop");
        card_manager.InvokeBurnAndFlop();
    }

    private IEnumerator RunSecondBet()
    {
        Debug.Log("RunSecondBet");
        betting_manager.ResetBets();
        betting_manager.StartRound(0);
        while (!betting_manager.CheckIfEveryoneIsDone())
        {
            yield return null;
        }
        betting_manager.DisableAllBetting();
        SetGameState(GameState.Turn);
    }

    private void RunTurn()
    {
        Debug.Log("RunTurn");
        card_manager.InvokeBurnAndTurn();
    }

    private IEnumerator RunThirdBet()
    {
        Debug.Log("RunThirdBet");
        betting_manager.ResetBets();
        betting_manager.StartRound(0);
        while (!betting_manager.CheckIfEveryoneIsDone())
        {
            yield return null;
        }
        betting_manager.DisableAllBetting();
        SetGameState(GameState.River);
    }

    private void RunRiver()
    {
        Debug.Log("RunRiver");
        card_manager.InvokeBurnAndRiver();
    }

    private IEnumerator RunFourthBet()
    {
        Debug.Log("RunFourthBet");
        betting_manager.ResetBets();
        betting_manager.StartRound(0);
        while (!betting_manager.CheckIfEveryoneIsDone())
        {
            yield return null;
        }
        betting_manager.DisableAllBetting();
        SetGameState(GameState.Payout);
    }

    private void RunPayout()
    {
        Debug.Log("RunPayout");
        payout_manager.RunPayout();
        SetGameState(GameState.Reset);
    }

    private void RunReset()
    {
        Debug.Log("RunReset");
        card_manager.ResetTable();
        betting_manager.ResetBets();
        betting_manager.ResetHand();
        SetGameState(GameState.Deal);
    }
}
