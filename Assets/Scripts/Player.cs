using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar] public int ID = -1;
    [SyncVar] public int ChipCount;
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

    public string cardA;
    public string cardB;

    private GameManager game_manager;

    public bool getcards = false;

    // Use this for initialization
    void Start() {
        game_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        PotText = Canvas.transform.Find("Pot").GetComponent<Text>();
        ChipText = Canvas.transform.Find("Stack").GetComponent<Text>();
        CardAtext = Canvas.transform.Find("Card A").GetComponent<Text>();
        CardBtext = Canvas.transform.Find("Card B").GetComponent<Text>();
        Card1text = Canvas.transform.Find("Card 1").GetComponent<Text>();
        Card2text = Canvas.transform.Find("Card 2").GetComponent<Text>();
        Card3text = Canvas.transform.Find("Card 3").GetComponent<Text>();
        Card4text = Canvas.transform.Find("Card 4").GetComponent<Text>();
        Card5text = Canvas.transform.Find("Card 5").GetComponent<Text>();

        ID = game_manager.Register();
        ChipCount = 500;
        UpdateChipCount();

        cardA = "";
        cardB = "";

        gameObject.tag = "Player" + ID;
    }

    // Update is called once per frame
    void Update() {
        ShowPot();
        UpdateCardUI();
        UpdateTableCardsUI();
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
        ChipCount -= amt;
        UpdateChipCount();
        CmdBet(amt);
    }

    [Command]
    private void CmdBet(int amt) {
        game_manager.AddToPot(amt);
    }

    //~~~~~~~~~~~~~SETTERS~~~~~~~~~~~~~~//

    public void ClearCards()
    {
        cardA = "";
        cardB = "";
    }

    //~~~~~~~~~~~~~GETTERS~~~~~~~~~~~~~~//
    public int GetChipCount()
    {
        return ChipCount;
    }

    //~~~~~~~~~~~~~UI UPDATES~~~~~~~~~~~~~~//
    private void ShowPot() {
        PotText.text = "Pot: " + game_manager.GetPot().ToString();
    }

    private void UpdateChipCount() {
        ChipText.text = "Chip Stack: " + ChipCount.ToString();
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

}
