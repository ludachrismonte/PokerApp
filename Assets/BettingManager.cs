﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BettingManager : NetworkBehaviour {

    private GameManager game_manager;

    [SyncVar] private int current_bet;
    [SyncVar] private int current_pot;
    [SyncVar] public int turn_id;
    [SyncVar] private bool round_over;

    public List<Player> players;

    // Use this for initialization
    void Start() {
        game_manager = GetComponent<GameManager>();
        turn_id = 0;
        Debug.Log("Betting Manager started turn at player " + turn_id);
        Reset();
    }

    // Update is called once per frame
    void Update() {
        if (players.Count > 0 && !players[turn_id].IsIn() && players[turn_id].HasCalled()) {
            MoveToNextPlayer();
        }
    }

    public void InitializePlayerList(int num_registered) {
        for (int i = 0; i < num_registered; i++) {
            players.Add(GameObject.Find("Player" + i).GetComponent<Player>());
        }
    }

    private void Reset()
    {
        round_over = false;
        current_bet = 0;
        current_pot = 0;
    }

    public int GetTurnID() {
        return turn_id;
    }

    private void MoveToNextPlayer() {
        turn_id++;
        if (turn_id == game_manager.GetNumRegistered()) {
            turn_id = 0;
        }
        Debug.Log("Betting Manager moved turn to player " + turn_id);
    }

    //~~~~~~~~~~~~POT FUNCTIONS~~~~~~~~~~~//


    public int GetPot()
    {
        return current_pot;
    }

    public void AddToPot(int amt)
    {
        current_pot += amt;
    }

    //~~~~~~~~~~~~~~ACTIONS~~~~~~~~~~~~~~//

    public bool ServerCall(int ID, int amount) {
        if (ID != turn_id) {
            Debug.LogError("Cannot Call, " + ID + " it's not your turn");
            return false;
        }

        current_pot += amount;
        MoveToNextPlayer();
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
            Debug.LogError("Must bet at least twice the current bet");
            return false;
        }

        current_bet = amount;
        current_pot += amount;
        MoveToNextPlayer();
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

        MoveToNextPlayer();
        return true;
    }
}