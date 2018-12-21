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

    private GameManager game_manager;

    // Use this for initialization
    void Start() {
        game_manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        PotText = Canvas.transform.Find("Pot").GetComponent<Text>();
        ChipText = Canvas.transform.Find("Stack").GetComponent<Text>();

        ID = game_manager.Register(this.gameObject);
        ChipCount = 500;
        UpdateChipCount();
    }

    // Update is called once per frame
    void Update() {
        ShowPot();
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
}
