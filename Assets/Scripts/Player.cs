using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar] public int ID = -1;

    public GameObject Canvas;

    private Text PotText;
    private Text ChipText;
    private Text CardAtext;
    private Text CardBtext;

    private Text Card1text;
    private Text Card2text;
    private Text Card3text;
    private Text Card4text;
    private Text Card5text;

    private Text Stack0;
    private Text Stack1;
    private Text Stack2;
    private Text Stack3;
    private Text Stack4;
    private Text Stack5;
    private Text Stack6;
    private Text Stack7;
    private Text Stack8;

    public string cardA;
    public string cardB;

    private GameManager game_manager;
    private StackManager stack_manager;

    public bool getcards = false;

    // Use this for initialization
    void Start() {
        game_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stack_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StackManager>();
        
        PotText = Canvas.transform.Find("Pot").GetComponent<Text>();
        ChipText = Canvas.transform.Find("My Stack").GetComponent<Text>();

        CardAtext = Canvas.transform.Find("Card A").GetComponent<Text>();
        CardBtext = Canvas.transform.Find("Card B").GetComponent<Text>();

        Card1text = Canvas.transform.Find("Card 1").GetComponent<Text>();
        Card2text = Canvas.transform.Find("Card 2").GetComponent<Text>();
        Card3text = Canvas.transform.Find("Card 3").GetComponent<Text>();
        Card4text = Canvas.transform.Find("Card 4").GetComponent<Text>();
        Card5text = Canvas.transform.Find("Card 5").GetComponent<Text>();

        Stack0 = Canvas.transform.Find("Stack 0").GetComponent<Text>();
        Stack1 = Canvas.transform.Find("Stack 1").GetComponent<Text>();
        Stack2 = Canvas.transform.Find("Stack 2").GetComponent<Text>();
        Stack3 = Canvas.transform.Find("Stack 3").GetComponent<Text>();
        Stack4 = Canvas.transform.Find("Stack 4").GetComponent<Text>();
        Stack5 = Canvas.transform.Find("Stack 5").GetComponent<Text>();
        Stack6 = Canvas.transform.Find("Stack 6").GetComponent<Text>();
        Stack7 = Canvas.transform.Find("Stack 7").GetComponent<Text>();
        Stack8 = Canvas.transform.Find("Stack 8").GetComponent<Text>();

        ID = game_manager.Register();
        stack_manager.SetStack(ID, 500);

        cardA = "";
        cardB = "";

        gameObject.tag = "Player" + ID;
    }

    // Update is called once per frame
    void Update() {
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

    //~~~~~~~~~~~~~BETTING~~~~~~~~~~~~~~//
    public void bet(int amt) {
        CmdBet(amt);
    }

    [Command]
    private void CmdBet(int amt) {
        game_manager.AddToPot(amt);
        stack_manager.ModStack(ID, -amt);
    }

    [Command]
    private void CmdUpdateStack()
    {

    }
    //~~~~~~~~~~~~~SETTERS~~~~~~~~~~~~~~//

    public void ClearCards()
    {
        cardA = "";
        cardB = "";
    }

    //~~~~~~~~~~~~~GETTERS~~~~~~~~~~~~~~//


    //~~~~~~~~~~~~~UI UPDATES~~~~~~~~~~~~~~//

    private void UpdateUI() {
        ShowPot();
        UpdateChipCountUI();
        UpdateCardUI();
        UpdateTableCardsUI();
        UpdateTableStacks();
    }

    private void ShowPot() {
        PotText.text = "Pot: " + game_manager.GetPot().ToString();
    }

    private void UpdateChipCountUI() {
        ChipText.text = "Chip Stack: " + stack_manager.GetStack(ID).ToString();
    }

    private void UpdateCardUI() {
        CardAtext.text = game_manager.GetCard(ID, 0);
        CardBtext.text = game_manager.GetCard(ID, 1);
    }

    private void UpdateTableCardsUI()
    {
        Card1text.text = game_manager.GetTableCard(1);
        Card2text.text = game_manager.GetTableCard(2);
        Card3text.text = game_manager.GetTableCard(3);
        Card4text.text = game_manager.GetTableCard(4);
        Card5text.text = game_manager.GetTableCard(5);
    }

    private void UpdateTableStacks()
    {
        List<int> Stacks = stack_manager.GetStackCounts();
        Stack0.text = (Stacks[0] > 0 && ID != 0) ? "Stack 0: " + Stacks[0].ToString() : "";
        Stack1.text = (Stacks[1] > 0 && ID != 1) ? "Stack 1: " + Stacks[1].ToString() : "";
        Stack2.text = (Stacks[2] > 0 && ID != 2) ? "Stack 2: " + Stacks[2].ToString() : "";
        Stack3.text = (Stacks[3] > 0 && ID != 3) ? "Stack 3: " + Stacks[3].ToString() : "";
        Stack4.text = (Stacks[4] > 0 && ID != 4) ? "Stack 4: " + Stacks[4].ToString() : "";
        Stack5.text = (Stacks[5] > 0 && ID != 5) ? "Stack 5: " + Stacks[5].ToString() : "";
        Stack6.text = (Stacks[6] > 0 && ID != 6) ? "Stack 6: " + Stacks[6].ToString() : "";
        Stack7.text = (Stacks[7] > 0 && ID != 7) ? "Stack 7: " + Stacks[7].ToString() : "";
        Stack8.text = (Stacks[8] > 0 && ID != 8) ? "Stack 8: " + Stacks[8].ToString() : "";
    }

}
