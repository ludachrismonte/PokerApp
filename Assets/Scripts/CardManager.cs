using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardManager : NetworkBehaviour {

    public Cards Deck;

    //Personal Cards
    [SyncVar] private Card CardA0;
    [SyncVar] private Card CardB0;
    [SyncVar] private Card CardA1;
    [SyncVar] private Card CardB1;
    [SyncVar] private Card CardA2;
    [SyncVar] private Card CardB2;
    [SyncVar] private Card CardA3;
    [SyncVar] private Card CardB3;
    [SyncVar] private Card CardA4;
    [SyncVar] private Card CardB4;
    [SyncVar] private Card CardA5;
    [SyncVar] private Card CardB5;
    [SyncVar] private Card CardA6;
    [SyncVar] private Card CardB6;
    [SyncVar] private Card CardA7;
    [SyncVar] private Card CardB7;
    [SyncVar] private Card CardA8;
    [SyncVar] private Card CardB8;

    //Shared Cards
    [SyncVar] private Card Card1;
    [SyncVar] private Card Card2;
    [SyncVar] private Card Card3;
    [SyncVar] private Card Card4;
    [SyncVar] private Card Card5;

    private GameManager game_manager;

    // Use this for initialization
    void Start () {
        ClearTable();
        game_manager = GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator ShuffleAndDeal()
    {
        Deck.Shuffle();
        for (int round = 1; round <= 2; round++)
        {
            for (int i = 0; i < game_manager.GetNumRegistered(); i++)
            {
                FileCard(Deck.GetTopCard(), i, round);
            }
            //Wait for network latency
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < game_manager.GetNumRegistered(); i++)
            {
                GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateCardUI();
            }
        }
    }

    public string GetCard(int ID, int which)
    {
        switch (ID)
        {
            case 0:
                return (which == 0) ? CardA0.String() : CardB0.String();
            case 1:
                return (which == 0) ? CardA1.String() : CardB1.String();
        }
        return "ERROR";
    }

    public string GetTableCard(int which)
    {
        switch (which)
        {
            case 0:
                return Card1.String();
            case 1:
                return Card2.String();
            case 2:
                return Card3.String();
            case 3:
                return Card4.String();
            case 4:
                return Card5.String();
        }
        return "ERROR";
    }

    private void FileCard(Card card, int ID, int round)
    {
        switch (ID)
        {
            case 0:
                if (round == 1) { CardA0 = card; }
                else CardB0 = card;
                break;
            case 1:
                if (round == 1) { CardA1 = card; }
                else CardB1 = card;
                break;
        }
    }

    private void ClearTable()
    {
        Card1 = Deck.GetBlankCard();
        Card2 = Deck.GetBlankCard();
        Card3 = Deck.GetBlankCard();
        Card4 = Deck.GetBlankCard();
        Card5 = Deck.GetBlankCard();
    }

    //~~~~~~~~~~~~~~Stage Coroutines~~~~~~~~~~~~//

    public IEnumerator BurnAndFlop()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
        //Deal
        Card1 = Deck.GetTopCard();
        Debug.Log("Dealing 1: " + Card1.String());
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
        Card2 = Deck.GetTopCard();
        Debug.Log("Dealing 2: " + Card2.String());
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
        Card3 = Deck.GetTopCard();
        Debug.Log("Dealing 3: " + Card3.String());
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
    }

    public IEnumerator BurnAndTurn()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        Card4 = Deck.GetTopCard();
        Debug.Log("Dealing 4: " + Card4.String());
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
    }

    public IEnumerator BurnAndRiver()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        Card5 = Deck.GetTopCard();
        Debug.Log("Dealing 5: " + Card5.String());
        for (int i = 0; i < game_manager.GetNumRegistered(); i++)
        {
            GameObject.Find("Player" + i).GetComponent<Player>().RpcUpdateTableCardsUI();
        }
    }
}
