using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

public class GameManager : NetworkBehaviour {

    private bool Proceed = false;

    private GameState game_state;

    [SyncVar] public int num_registered;
    [SyncVar] private int dealer_id;

    private CardManager card_manager;
    private BettingManager betting_manager;

    // Use this for initialization
    void Start () {
        num_registered = 0;
        card_manager = GetComponent<CardManager>();
        betting_manager = GetComponent<BettingManager>();
        Debug.Log("Waiting for players to start...");
    }

    // Control Gameloop
    void Update () {
        switch (game_state)
        {   
            case GameState.Deal:
                game_state = GameState.WaitingLimbo;
                RunDeal();
                break;
            case GameState.FirstBet:
                game_state = GameState.BettingLimbo;
                StartCoroutine(RunFirstBet());
                break;
            case GameState.Flop:
                game_state = GameState.WaitingLimbo;
                RunFlop();
                break;
            case GameState.SecondBet:
                game_state = GameState.BettingLimbo;
                StartCoroutine(RunSecondBet());
                break;
            case GameState.Turn:
                game_state = GameState.WaitingLimbo;
                RunTurn();
                break;
            case GameState.ThirdBet:
                game_state = GameState.BettingLimbo;
                StartCoroutine(RunThirdBet());
                break;
            case GameState.River:
                game_state = GameState.WaitingLimbo;
                RunRiver();
                break;
            case GameState.FourthBet:
                game_state = GameState.BettingLimbo;
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

    public int Register() {
        num_registered++;
        return num_registered - 1;
    }

    public int GetNumRegistered() {
        return num_registered;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATE SETS~~~~~~~~~~~~~~~~~~~~~~//

    public void StartGame()
    {
        betting_manager.InitializePlayerList(num_registered);
        SetGameState(GameState.Deal);
    }

    public void SetGameState(GameState state) {
        game_state = state;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATE CHECKS~~~~~~~~~~~~~~~~~~~~~~//

    public bool IsOnBettingStage() {
        return game_state == GameState.BettingLimbo;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~GAME STATES~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void RunDeal() {
        Debug.Log("Dealing");
        StartCoroutine(card_manager.ShuffleAndDeal());
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
        game_state = GameState.Flop;
    }

    private void RunFlop()
    {
        Debug.Log("RunFlop");
        StartCoroutine(card_manager.BurnAndFlop());
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
        game_state = GameState.Turn;
    }

    private void RunTurn()
    {
        Debug.Log("RunTurn");
        StartCoroutine(card_manager.BurnAndTurn());
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
        game_state = GameState.River;
    }

    private void RunRiver()
    {
        Debug.Log("RunRiver");
        StartCoroutine(card_manager.BurnAndRiver());
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
        betting_manager.ResetBets();
        betting_manager.ResetHand();
    }
}
