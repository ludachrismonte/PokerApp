using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar] public int ID = -1;

    public GameObject Canvas;
    public Image[] TableCards;
    public Text[] TableStacks;

    private Text PotText;
    private Text ChipText;

    private Image CardA;
    private Image CardB;
    private Dictionary<string, Sprite> Sprites;

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

        LoadSpriteDictionary();
        CardA = Canvas.transform.Find("Card A").GetComponent<Image>();
        CardB = Canvas.transform.Find("Card B").GetComponent<Image>();

        ID = game_manager.Register();
        stack_manager.SetStack(ID, 500);

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
        Debug.Log("Getting: " + card_manager.GetCard(ID, 0));
        CardA.sprite = GetSpriteByName(card_manager.GetCard(ID, 0));
        CardB.sprite = GetSpriteByName(card_manager.GetCard(ID, 1));
    }

    private void UpdateTableCardsUI()
    {
        for (int i = 0; i < 5; i++) {
            TableCards[i].sprite = GetSpriteByName(card_manager.GetTableCard(i));
        }
    }

    private void UpdateTableStacks()
    {
        for (int i = 0; i < 9; i++) {
            TableStacks[i].text = (stack_manager.GetStack(i) > 0 && ID != 0) ? "Stack " + i + ": " + stack_manager.GetStack(i).ToString() : "";
        }
    }

    //~~~~~~~~~~~~~SPRITE CONTROL~~~~~~~~~~~~~~//


    private void LoadSpriteDictionary()
    {
        Sprite[] SpritesData = Resources.LoadAll<Sprite>("cards");
        Sprites = new Dictionary<string, Sprite>();

        for (int i = 0; i < SpritesData.Length; i++)
        {
            Sprites.Add(SpritesData[i].name, SpritesData[i]);
        }
    }

    private Sprite GetSpriteByName(string name)
    {
        if (Sprites.ContainsKey(name))
            return Sprites[name];
        else
            return null;
    }
}
