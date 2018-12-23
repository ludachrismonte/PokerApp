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

    private bool GameStart = false;

    private GameState game_state;

    [SyncVar] private int num_registered;
    [SyncVar] private int current_pot;
    [SyncVar] private int current_bet;
    [SyncVar] private int dealer_id;

    private CardManager card_manager;

    // Use this for initialization
    void Start () {
        num_registered = 0;
        game_state = GameState.PreGame;
        card_manager = GetComponent<CardManager>();
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
            card_manager.Deal();
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
            StartCoroutine(card_manager.BurnAndFlop());
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
            StartCoroutine(card_manager.BurnAndTurn());
            game_state = GameState.ThirdBet;
        }
        if (game_state == GameState.ThirdBet)
        {
            game_state = GameState.River;
        }
        if (game_state == GameState.River)
        {
            StartCoroutine(card_manager.BurnAndRiver());
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

    public int GetNumRegistered() {
        return num_registered;
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

}
