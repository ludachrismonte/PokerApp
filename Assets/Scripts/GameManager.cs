using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum GameState
{
    PreGame = 1,
    Deal = 2,
    FirstBet = 3,
    Flop = 4,
    SecondBet = 5,
    Turn = 6,
    ThirdBet = 7,
    River = 8,
    FourthBet = 9,
    Payout = 10,
    Reset = 11
};

public class GameManager : NetworkBehaviour {

    public Cards Deck;

    private bool GameStart = false;

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

    private GameState game_state;

    [SyncVar] private int num_registered;
    [SyncVar] private int current_pot;
    [SyncVar] private int current_bet;
    [SyncVar] private int dealer_id;

    // Use this for initialization
    void Start () {
        num_registered = 0;
        game_state = GameState.PreGame;
        ClearTable();

    }
	
	// Control Gameloop
	void Update () {
        if (game_state == GameState.PreGame && GameStart) {
            GameStart = false;
            Debug.Log("Starting Game");
            game_state = GameState.Deal;
        }
        if (game_state == GameState.Deal) {
            Debug.Log("Dealing Hands");
            Deal();
            game_state = GameState.FirstBet;
        }
        if (game_state == GameState.FirstBet)
        {
            Debug.Log("First Round of Betting");
            game_state = GameState.Flop;
        }
        if (game_state == GameState.Flop)
        {
            Debug.Log("Dealing Flop");
            StartCoroutine(BurnAndFlop());
            game_state = GameState.SecondBet;
        }
        if (game_state == GameState.SecondBet)
        {
            Debug.Log("Second Round of Betting");
            game_state = GameState.Turn;
        }
        if (game_state == GameState.Turn)
        {
            Debug.Log("Dealing Turn");
            StartCoroutine(BurnAndTurn());
            game_state = GameState.ThirdBet;
        }
        if (game_state == GameState.ThirdBet)
        {
            game_state = GameState.River;
        }
        if (game_state == GameState.River)
        {
            StartCoroutine(BurnAndRiver());
            game_state = GameState.FourthBet;
        }
        if (game_state == GameState.FourthBet)
        {
            game_state = GameState.Payout;
        }
        if (game_state == GameState.Payout)
        {
            game_state = GameState.Reset;
        }
        if (game_state == GameState.Reset)
        {
            game_state = GameState.PreGame;
        }
    }

    public int Register() {
        num_registered++;
        return num_registered - 1;
    }

    public void StartGame() {
        GameStart = true;
    }

    public int GetPot() {
        return current_pot;
    }

    public void AddToPot(int amt) {
        current_pot += amt;
    }

    public void Deal() {
        for (int round = 1; round <= 2; round++)
        {
            for (int i = 0; i < num_registered; i++)
            {
                FileCard(Deck.GetTopCard(), i, round);
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
            case 1:
                return Card1.String();
            case 2:
                return Card2.String();
            case 3:
                return Card3.String();
            case 4:
                return Card4.String();
            case 5:
                return Card5.String();
        }
        return "ERROR";
    }

    private void FileCard(Card card, int ID, int round) {
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

    private void ClearTable() {
        Card1 = Deck.GetBlankCard();
        Card2 = Deck.GetBlankCard();
        Card3 = Deck.GetBlankCard();
        Card4 = Deck.GetBlankCard();
        Card5 = Deck.GetBlankCard();
    }

    //~~~~~~~~~~~~~~Stage Coroutines~~~~~~~~~~~~//

    private IEnumerator BurnAndFlop() {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        Card1 = Deck.GetTopCard();
        Debug.Log("Dealing 1: " + Card1.String());
        yield return new WaitForSeconds(1f);
        Card2 = Deck.GetTopCard();
        Debug.Log("Dealing 2: " + Card2.String());
        yield return new WaitForSeconds(1f);
        Card3 = Deck.GetTopCard();
        Debug.Log("Dealing 3: " + Card3.String());
    }

    private IEnumerator BurnAndTurn()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        Card4 = Deck.GetTopCard();
        Debug.Log("Dealing 4: " + Card4.String());
    }

    private IEnumerator BurnAndRiver()
    {
        //Burn
        Card burn = Deck.GetTopCard();
        Debug.Log("Burning " + burn.String());
        yield return new WaitForSeconds(1f);
        //Deal
        Card5 = Deck.GetTopCard();
        Debug.Log("Dealing 5: " + Card5.String());
    }
}
