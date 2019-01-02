using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardManager : NetworkBehaviour {

    public Cards Deck;

    //Shared Cards
    private Card[] TableCards;

    private GameManager game_manager;

    // Use this for initialization
    void Start () {
        TableCards = new Card[5];
        game_manager = GetComponent<GameManager>();
        ClearTable();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public IEnumerator ShuffleAndDeal()
    {
        Deck.Shuffle();
        for (int card_round = 0; card_round <= 1; card_round++)
        {
            for (int i = 0; i < game_manager.GetNumRegistered(); i++)
            {
                GameObject.Find("Player" + i).GetComponent<Player>().RpcGetCardFromServer(Deck.GetTopCard(), card_round);
            }
            yield return new WaitForSeconds(1);
        }
        game_manager.SetGameState(GameState.FirstBet);
    }

    public void ClearTable()
    {
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i.ToString()).GetComponent<Player>().RpcGetCardFromServer(Deck.GetBlankCard(), 0);
            GameObject.Find("Player" + i.ToString()).GetComponent<Player>().RpcGetCardFromServer(Deck.GetBlankCard(), 1);
        }
        for (int p = 0; p < game_manager.GetNumRegistered(); p++)
        {
            GameObject.Find("Player" + p.ToString()).GetComponent<Player>().RpcGetTableCardFromServer(Deck.GetBlankCard(), 0);
            GameObject.Find("Player" + p.ToString()).GetComponent<Player>().RpcGetTableCardFromServer(Deck.GetBlankCard(), 1);
            GameObject.Find("Player" + p.ToString()).GetComponent<Player>().RpcGetTableCardFromServer(Deck.GetBlankCard(), 2);
            GameObject.Find("Player" + p.ToString()).GetComponent<Player>().RpcGetTableCardFromServer(Deck.GetBlankCard(), 3);
            GameObject.Find("Player" + p.ToString()).GetComponent<Player>().RpcGetTableCardFromServer(Deck.GetBlankCard(), 4);
        }
        for (int i = 0; i < 5; i++) {
            TableCards[i] = Deck.GetBlankCard();
        }
    }

    //~~~~~~~~~~~~~~Stage Coroutines~~~~~~~~~~~~//

    public IEnumerator BurnAndFlop()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        //Deal
        for (int r = 0; r < 3; r++) {
            TableCards[r] = Deck.GetTopCard();
            Debug.Log("Dealing " + (r+1).ToString() + ": " + TableCards[r].String());
            for (int i = 0; i < game_manager.GetNumRegistered(); i++)
            {
                GameObject.Find("Player" + i).GetComponent<Player>().RpcGetTableCardFromServer(TableCards[r], r);
            }
            yield return new WaitForSeconds(1);
        }
        game_manager.SetGameState(GameState.SecondBet);
    }

    public IEnumerator BurnAndTurn()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        TableCards[3] = Deck.GetTopCard();
        Debug.Log("Dealing 4: " + TableCards[3].String());
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcGetTableCardFromServer(TableCards[3], 3);
        }
        game_manager.SetGameState(GameState.ThirdBet);
    }

    public IEnumerator BurnAndRiver()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        TableCards[4] = Deck.GetTopCard();
        Debug.Log("Dealing 5: " + TableCards[4].String());
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcGetTableCardFromServer(TableCards[4], 4);
        }
        game_manager.SetGameState(GameState.FourthBet);
    }
}
