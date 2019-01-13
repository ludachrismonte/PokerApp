using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTabsManager : MonoBehaviour {

    public static PlayerTabsManager player_tabs_manager;

    private GameObject LocalPlayer;

    public RectTransform[] TabLocations;

    private void Awake()
    {
        if (PlayerTabsManager.player_tabs_manager == null)
        {
            PlayerTabsManager.player_tabs_manager = this;
        }
        else
        {
            if (PlayerTabsManager.player_tabs_manager != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetLocalPlayer(GameObject p) {
        LocalPlayer = p;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void AssignLocations() {
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++) {
            GameManager.game_manager.AllPlayers[i].GetPublicUI().GetComponent<RectTransform>().position = TabLocations[i].position;
        }
    }
}
