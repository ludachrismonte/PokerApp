using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar] public int ID = -1;

    public GameObject Canvas;
    public Text[] TableCardTexts;
    public Text[] TableStacks;

    private Text PotText;
    private Text ChipText;
    private Text CardAtext;
    private Text CardBtext;

    public string cardA;
    public string cardB;

    private GameManager game_manager;
    private StackManager stack_manager;
    private CardManager card_manager;
    private BettingManager betting_manager;

    [SyncVar] private bool Is_In_Hand;
    [SyncVar] private bool Bets_Are_Up_To_Date;

    // Use this for initialization
    void Start() {
        game_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stack_manager = game_manager.gameObject.GetComponent<StackManager>();
        card_manager = game_manager.gameObject.GetComponent<CardManager>();
        betting_manager = game_manager.gameObject.GetComponent<BettingManager>();


        PotText = Canvas.transform.Find("Pot").GetComponent<Text>();
        ChipText = Canvas.transform.Find("My Stack").GetComponent<Text>();

        CardAtext = Canvas.transform.Find("Card A").GetComponent<Text>();
        CardBtext = Canvas.transform.Find("Card B").GetComponent<Text>();

        ID = game_manager.Register();
        stack_manager.SetStack(ID, 500);

        cardA = "";
        cardB = "";

        transform.name = "Player" + ID;

        Is_In_Hand = true;
    }

    // Update is called once per frame
    void Update() {
        if (game_manager.IsOnBettingStage() && betting_manager.GetTurnID() == ID) {
            Debug.Log("I am player " + ID + " and its my turn to bet!");
        }

        UpdateUI();
    }

    //~~~~~~~~~~~~~Ready-Up~~~~~~~~~~~~~~//
    public void Ready()
    {
        CmdReady();
    }

    [Command]
    private void CmdReady()
    {
        game_manager.StartGame();
    }

    //~~~~~~~~~~~~~GETTERS~~~~~~~~~~~~~~//

    public bool IsIn()
    {
        return Is_In_Hand;
    }

    public bool HasCalled()
    {
        return Bets_Are_Up_To_Date;
    }

    //~~~~~~~~~~~~~SETTERS~~~~~~~~~~~~~~//

    public void ClearCards()
    {
        cardA = "";
        cardB = "";
    }

    //~~~~~~~~~~~~~BETTING~~~~~~~~~~~~~~//
    public void bet(int amt) {
        if (amt > stack_manager.GetStack(ID)) {
            Debug.Log("Can't bet more than my stack");
        }
        Bets_Are_Up_To_Date = true;
        CmdBet(amt);
    }

    [Command]
    private void CmdBet(int amt) {
        betting_manager.ServerBet(ID, amt);
        stack_manager.ModStack(ID, -amt);
    }

    //~~~~~~~~~~~~~FOLDING~~~~~~~~~~~~~~//

    public void Fold()
    {
        CmdFold();
    }

    [Command]
    private void CmdFold()
    {
        Is_In_Hand = false;
    }

    //~~~~~~~~~~~~~UI UPDATES~~~~~~~~~~~~~~//

    private void UpdateUI() {
        ShowPot();
        UpdateChipCountUI();
        UpdateCardUI();
        UpdateTableCardsUI();
        UpdateTableStacks();
    }

    private void ShowPot() {
        PotText.text = "Pot: " + betting_manager.GetPot().ToString();
    }

    private void UpdateChipCountUI() {
        ChipText.text = "Chip Stack: " + stack_manager.GetStack(ID).ToString();
    }

    private void UpdateCardUI() {
        CardAtext.text = card_manager.GetCard(ID, 0);
        CardBtext.text = card_manager.GetCard(ID, 1);
    }

    private void UpdateTableCardsUI()
    {
        for (int i = 0; i < 5; i++) {
            TableCardTexts[i].text = card_manager.GetTableCard(i);
        }
    }

    private void UpdateTableStacks()
    {
        for (int i = 0; i < 9; i++) {
            TableStacks[i].text = (stack_manager.GetStack(i) > 0 && ID != 0) ? "Stack " + i + ": " + stack_manager.GetStack(i).ToString() : "";
        }
    }

}
