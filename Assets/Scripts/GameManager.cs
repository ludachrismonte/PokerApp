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

    private bool Proceed = false;

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
        switch (game_state)
        {
            case GameState.PreGame:
                RunPregame();
                break;
            case GameState.Deal:
                RunDeal();
                break;
            case GameState.FirstBet:
                RunFirstBet();
                break;
            case GameState.Flop:
                RunFlop();
                break;
            case GameState.SecondBet:
                RunSecondBet();
                break;
            case GameState.Turn:
                RunTurn();
                break;
            case GameState.ThirdBet:
                RunThirdBet();
                break;
            case GameState.River:
                RunRiver();
                break;
            case GameState.FourthBet:
                RunFourthBet();
                break;
            case GameState.Payout:
                RunPayout();
                break;
            case GameState.Reset:
                RunReset();
                break;
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
        Proceed = true;
    }

    public int GetPot() {
        return current_pot;
    }

    public void AddToPot(int amt) {
        current_pot += amt;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATES~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void RunPregame() {
        if (!Proceed) {
            return;
        }
        Debug.Log("Starting Game");
        game_state = GameState.Deal;
        Proceed = false;
    }

    private void RunDeal() {
        if (!Proceed)
        {
            return;
        }
        Debug.Log("Dealing");
        card_manager.Deal();
        game_state = GameState.FirstBet;
        Proceed = false;
    }

    private void RunFirstBet()
    {
        Debug.Log("RunFirstBet");
        game_state = GameState.Flop;
    }

    private void RunFlop()
    {
        if (!Proceed)
        {
            return;
        }
        Debug.Log("RunFlop");
        StartCoroutine(card_manager.BurnAndFlop());
        game_state = GameState.SecondBet;
        Proceed = false;
    }

    private void RunSecondBet()
    {
        Debug.Log("RunSecondBet");
        game_state = GameState.Turn;
    }

    private void RunTurn()
    {
        if (!Proceed)
        {
            return;
        }
        Debug.Log("RunTurn");
        StartCoroutine(card_manager.BurnAndTurn());
        game_state = GameState.ThirdBet;
        Proceed = false;
    }

    private void RunThirdBet()
    {
        Debug.Log("RunThirdBet");
        game_state = GameState.River;
    }

    private void RunRiver()
    {
        if (!Proceed)
        {
            return;
        }
        Debug.Log("RunRiver");
        StartCoroutine(card_manager.BurnAndRiver());
        game_state = GameState.FourthBet;
        Proceed = false;
    }

    private void RunFourthBet()
    {
        Debug.Log("RunFourthBet");
        game_state = GameState.Payout;
    }

    private void RunPayout()
    {
        Debug.Log("RunPayout");
        game_state = GameState.Reset;
    }

    private void RunReset()
    {
        Debug.Log("RunReset");
        game_state = GameState.PreGame;
    }

}
