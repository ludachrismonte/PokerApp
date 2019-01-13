using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PokerPlayer : MonoBehaviour {

    private PhotonView PV;

    public int ID = -1;

    public GameObject PrivateCanvas;
    public GameObject PublicCanvas;
    private GameObject TurnObjects;

    public Text[] TableStacks;
    private Text ChipText;

    private Image[] MyCardSprites;

    private Card[] MyCards;
    private Hand MyHandValue;

    private BettingManager betting_manager;
    private HandDeterminer hand_determiner;

    private int MyStack;

    private int my_bet;
    public BetSlider slider;

    private bool Is_In_Hand;
    public bool Bets_Are_Up_To_Date;

    private float SlowConnectionTimer;

    // Use this for initialization
    void Start() {
        PV = GetComponent<PhotonView>();

        betting_manager = GameManager.game_manager.GetComponent<BettingManager>();
        hand_determiner = GetComponent<HandDeterminer>();

        TurnObjects = PrivateCanvas.transform.Find("TurnObjects").gameObject;
        slider = TurnObjects.transform.Find("Slider").gameObject.GetComponent<BetSlider>();
        TurnObjects.SetActive(false);

        ChipText = PublicCanvas.transform.Find("My Stack").GetComponent<Text>();
        ChipText.text = "";

        MyCards = new Card[2];
        MyHandValue = new Hand(HandValue.None);

        MyCardSprites = new Image[2];
        MyCardSprites[0] = PublicCanvas.transform.Find("Card A").GetComponent<Image>();
        MyCardSprites[1] = PublicCanvas.transform.Find("Card B").GetComponent<Image>();

        Is_In_Hand = false;

        SlowConnectionTimer = 0f;

        PublicCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        SlowConnectionTimer += Time.deltaTime;
        if (SlowConnectionTimer > 1)
        {
            SlowConnectionTimer = 0f;
        }
        if (MyHandValue.type != HandValue.None) {
            Debug.Log(MyHandValue.String());
        }
    }

    //~~~~~~~~~~~~~Ready-Up~~~~~~~~~~~~~~//

    public void SitDown(string name, int stack_size)
    {
        PV.RPC("RPC_SitDown", RpcTarget.AllBuffered, name, stack_size);
    }

    [PunRPC]
    private void RPC_SitDown(string name, int stack_size)
    {
        ID = GameManager.game_manager.Register(this);
        transform.name = "Player" + ID;
        PublicCanvas.SetActive(true);
        PublicCanvas.transform.Find("My Name").GetComponent<Text>().text = name;
        SetStack(stack_size);
    }

    //~~~~~~~~~~~~~~GETTERS~~~~~~~~~~~~~~//

    public int GetID()
    {
        return ID;
    }

    public bool IsIn()
    {
        return Is_In_Hand;
    }

    public bool HasCalled()
    {
        return Is_In_Hand && Bets_Are_Up_To_Date;
    }

    public int GetStack() {
        return MyStack;
    }

    public Hand GetHand() {
        return MyHandValue;
    }

    public GameObject GetPublicUI()
    {
        return PublicCanvas;
    }

    //~~~~~~~~~~~~~SETTERS~~~~~~~~~~~~~~//
    [PunRPC]
    private void RPC_Set_IsIn(bool status)
    {
        Is_In_Hand = status;
    }

    [PunRPC]
    private void RPC_Set_IsUpToDate(bool status)
    {
        Bets_Are_Up_To_Date = status;
    }

    public void SetStack(int amt)
    {
        PV.RPC("RPC_SetStack", RpcTarget.All, amt);
    }

    public void ModStack(int amt)
    {
        int new_stack = MyStack + amt;
        PV.RPC("RPC_SetStack", RpcTarget.All, new_stack);
    }

    [PunRPC]
    private void RPC_SetStack(int amt)
    {
        MyStack = amt;
        ChipText.text = "Stack Size: " + MyStack.ToString();
    }

    public void RpcEnableBetting(bool on)
    {
        TurnObjects.SetActive(on);
    }

    public void GetCardFromServer(Card card, int which) {
        Debug.Log("Client is adding " + card.String() + " to thier hand's position " + which.ToString());
        MyCards[which] = card;
        if (card.String() == "NoneNone")
        {
            MyCardSprites[which].enabled = false;
        }
        else
        {
            MyCardSprites[which].enabled = true;
            if (PV.IsMine) {
                MyCardSprites[which].sprite = SpriteManager.sprite_manager.GetSpriteByName(card.String());
            }
            else MyCardSprites[which].sprite = SpriteManager.sprite_manager.GetSpriteByName("NoneNone");
        }
        if (which == 0) {
            hand_determiner.Clear();
        }
    }

    public void DeterminePlayerHand() {
        if (PV.IsMine) {
            MyHandValue = hand_determiner.Determine(MyCards);
        }
    }

    //~~~~~~~~~~~~~BETTING~~~~~~~~~~~~~~//
    public void ResetBets() {
        PV.RPC("RPC_Set_IsIn", RpcTarget.All, true);
        PV.RPC("RPC_Set_IsUpToDate", RpcTarget.All, false);
        slider.SetValueTo(0);
    }

    public void SetMyBet(int amt) {
        my_bet = amt;
    }

    public void bet() {
        if (my_bet > MyStack) {
            Debug.Log("Can't bet more than my stack");
        }
        if (betting_manager.ServerBet(ID, my_bet))
        {
            PV.RPC("RPC_Set_IsUpToDate", RpcTarget.All, true);
            ModStack(-my_bet);
        }
    }

    public void call()
    {
        if (my_bet > MyStack)
        {
            Debug.Log("Can't bet more than my stack");
        }
        if (betting_manager.ServerCall(ID))
        {
            PV.RPC("RPC_Set_IsUpToDate", RpcTarget.All, true);
            ModStack(-betting_manager.GetCurrentBet());
        }
    }

    //~~~~~~~~~~~~~FOLDING~~~~~~~~~~~~~~//

    public void Fold()
    {
        MyCardSprites[0].color = new Color(MyCardSprites[0].color.r, MyCardSprites[0].color.g, MyCardSprites[0].color.b, 0.75f);
        MyCardSprites[1].color = new Color(MyCardSprites[1].color.r, MyCardSprites[1].color.g, MyCardSprites[1].color.b, 0.75f);
        PV.RPC("RPC_Set_IsIn", RpcTarget.All, false);
    }

}
