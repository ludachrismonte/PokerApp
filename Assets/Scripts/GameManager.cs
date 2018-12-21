using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    [SyncVar] private int num_registered;
    [SyncVar] private int current_pot;
    [SyncVar] private int current_bet;


	// Use this for initialization
	void Start () {
        num_registered = 0;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public int Register(GameObject new_player) {
        num_registered++;
        return num_registered - 1;
    }

    public int GetPot() {
        return current_pot;
    }

    public void AddToPot(int amt) {
        current_pot += amt;
    }
}
