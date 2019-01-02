using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar] public int ID = -1;

    public GameObject Canvas;
    private GameObject TurnObjects;

    public Text[] TableStacks;
    private Text PotText;
    private Text ChipText;

    public Image[] TableCardSprites;
    private Image[] MyCardSprites;
    private Dictionary<string, Sprite> Sprites;

    private Card[] MyCards;
    private Hand MyHandValue;

    private GameManager game_manager;
    private StackManager stack_manager;
    private CardManager card_manager;
    private BettingManager betting_manager;

    private int my_bet;

    [SyncVar] private bool Is_In_Hand;
    [SyncVar] private bool Bets_Are_Up_To_Date;

    private float SlowConnectionTimer;

    // Use this for initialization
    void Start() {
        game_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stack_manager = game_manager.gameObject.GetComponent<StackManager>();
        card_manager = game_manager.gameObject.GetComponent<CardManager>();
        betting_manager = game_manager.gameObject.GetComponent<BettingManager>();
        TurnObjects = Canvas.transform.Find("TurnObjects").gameObject;
        TurnObjects.SetActive(false);
        PotText = Canvas.transform.Find("Pot").GetComponent<Text>();
        ChipText = Canvas.transform.Find("My Stack").GetComponent<Text>();

        MyCards = new Card[7];

        MyCardSprites = new Image[2];
        LoadSpriteDictionary();
        MyCardSprites[0] = Canvas.transform.Find("Card A").GetComponent<Image>();
        MyCardSprites[1] = Canvas.transform.Find("Card B").GetComponent<Image>();

        ID = game_manager.Register();
        stack_manager.SetStack(ID, 500);

        transform.name = "Player" + ID;

        Is_In_Hand = true;

        UpdateUI();

        SlowConnectionTimer = 0f;
    }

    // Update is called once per frame
    void Update() {
        SlowConnectionTimer += Time.deltaTime;
        if (SlowConnectionTimer > 1)
        {
            SlowConnectionTimer = 0f;
            UpdateUI();
        }
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
        return Is_In_Hand && Bets_Are_Up_To_Date;
    }

    public int GetStack() {
        return stack_manager.GetStack(ID);
    }

    //~~~~~~~~~~~~~SETTERS~~~~~~~~~~~~~~//

    [ClientRpc]
    public void RpcEnableBetting(bool on)
    {
        TurnObjects.SetActive(on);
    }

    [ClientRpc]
    public void RpcGetCardFromServer(Card card, int which) {
        Debug.Log("Client is adding " + card.String() + " to thier hand's position " + which.ToString());
        MyCards[which] = card;
        if (card.String() == "NoneNone")
        {
            MyCardSprites[which].enabled = false;
        }
        else
        {
            MyCardSprites[which].enabled = true;
            MyCardSprites[which].sprite = GetSpriteByName(card.String());
        }
    }

    [ClientRpc]
    public void RpcGetTableCardFromServer(Card card, int which)
    {
        Debug.Log("Client is adding " + card.String() + " to table position " + which.ToString());
        MyCards[which + 2] = card;
        if (card.String() == "NoneNone")
        {
            TableCardSprites[which].enabled = false;
        }
        else
        {
            TableCardSprites[which].enabled = true;
            TableCardSprites[which].sprite = GetSpriteByName(card.String());
        }
        if (card.String() != "NoneNone" && which > 1) {
            string s = "My Hand\n";
            for (int i = 0; i < MyCards.Length; i++)
            {
                s += MyCards[i].String() + "\n";
            }
            Debug.Log(s);
            MyHandValue = GetComponent<HandDeterminer>().Determine(MyCards);
        }
    }
    

    //~~~~~~~~~~~~~BETTING~~~~~~~~~~~~~~//
    public void ResetBets() {
        Bets_Are_Up_To_Date = false;
    }

    public void SetMyBet(int amt) {
        my_bet = amt;
    }

    public void bet() {
        if (my_bet > stack_manager.GetStack(ID)) {
            Debug.Log("Can't bet more than my stack");
        }
        Bets_Are_Up_To_Date = true;
        CmdBet();
    }

    [Command]
    private void CmdBet() {
        betting_manager.ServerBet(ID, my_bet);
        stack_manager.ModStack(ID, -my_bet);
    }

    //~~~~~~~~~~~~~FOLDING~~~~~~~~~~~~~~//

    public void Fold()
    {
        MyCardSprites[0].color = new Color(MyCardSprites[0].color.r, MyCardSprites[0].color.g, MyCardSprites[0].color.b, 0.75f);
        MyCardSprites[1].color = new Color(MyCardSprites[1].color.r, MyCardSprites[1].color.g, MyCardSprites[1].color.b, 0.75f);
        CmdFold();
    }

    [Command]
    private void CmdFold()
    {
        Is_In_Hand = false;
    }

    //~~~~~~~~~~~~~UI UPDATES~~~~~~~~~~~~~~//

    public void UpdateUI() {
        ShowPot();
        UpdateChipCountUI();
        UpdateTableStacks();
    }

    private void ShowPot() {
        PotText.text = "Pot: " + betting_manager.GetPot().ToString();
    }

    private void UpdateChipCountUI() {
        ChipText.text = "Chip Stack: " + stack_manager.GetStack(ID).ToString();
    }

    private void UpdateTableStacks()
    {
        for (int i = 0; i < 9; i++) {
            TableStacks[i].text = (stack_manager.GetStack(i) > 0 && ID != i) ? "Stack " + i + ": " + stack_manager.GetStack(i).ToString() : "";
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
