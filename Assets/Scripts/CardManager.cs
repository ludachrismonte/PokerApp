using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public Cards Deck;

    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void ShuffleAndDeal()
    {
        Deck.Shuffle();
        PV.RPC("RPC_Deal", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_Deal() {
        StartCoroutine(LocalDealCoroutine());
    }

    public IEnumerator LocalDealCoroutine()
    {
        for (int card_round = 0; card_round <= 1; card_round++)
        {
            for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
            {
                GameObject.Find("Player" + i).GetComponent<PokerPlayer>().GetCardFromServer(Deck.GetTopCard(), card_round);
                yield return new WaitForSeconds(.3f);
            }
        }
        DeterminePlayerHands();
        GameManager.game_manager.SetGameState(GameState.FirstBet);
    }

    public void ResetTable()
    {
        Deck.Reset();
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i.ToString()).GetComponent<PokerPlayer>().GetCardFromServer(Deck.GetBlankCard(), 0);
            GameObject.Find("Player" + i.ToString()).GetComponent<PokerPlayer>().GetCardFromServer(Deck.GetBlankCard(), 1);
        }
        TableCardsManager.TCManager.ResetTableCards();
    }

    private void DeterminePlayerHands() {
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<PokerPlayer>().DeterminePlayerHand();
        }
    }

    //~~~~~~~~~~~~~~Stage Coroutines~~~~~~~~~~~~//

    public void InvokeBurnAndFlop()
    {
        PV.RPC("RPC_BurnAndFlop", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_BurnAndFlop()
    {
        StartCoroutine(BurnAndFlopCoroutine());
    }

    public IEnumerator BurnAndFlopCoroutine()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        //Deal
        for (int r = 0; r < 3; r++) {
            TableCardsManager.TCManager.SetTableCard(Deck.GetTopCard(), r);
            DeterminePlayerHands();
            yield return new WaitForSeconds(1);
        }
        GameManager.game_manager.SetGameState(GameState.SecondBet);
    }

    //~~~~~~~~~~~//

    public void InvokeBurnAndTurn()
    {
        PV.RPC("RPC_BurnAndTurn", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_BurnAndTurn()
    {
        StartCoroutine(BurnAndTurnCoroutine());
    }

    public IEnumerator BurnAndTurnCoroutine()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        TableCardsManager.TCManager.SetTableCard(Deck.GetTopCard(), 3);
        DeterminePlayerHands();
        GameManager.game_manager.SetGameState(GameState.ThirdBet);
    }

    //~~~~~~~~~~~//

    public void InvokeBurnAndRiver()
    {
        PV.RPC("RPC_BurnAndRiver", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_BurnAndRiver()
    {
        StartCoroutine(BurnAndRiverCoroutine());
    }

    public IEnumerator BurnAndRiverCoroutine()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        TableCardsManager.TCManager.SetTableCard(Deck.GetTopCard(), 4);
        DeterminePlayerHands();
        GameManager.game_manager.SetGameState(GameState.FourthBet);
    }
}
