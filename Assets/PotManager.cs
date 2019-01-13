using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotManager : MonoBehaviour {

    public static PotManager pot_manager;
    private PhotonView PV;

    private int current_pot;
    private Text PotText;

    private void Awake()
    {
        if (PotManager.pot_manager == null)
        {
            PotManager.pot_manager = this;
        }
        else
        {
            if (PotManager.pot_manager != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Use this for initialization
    void Start () {
        PV = GetComponent<PhotonView>();
        PotText = GetComponent<Text>();
        SetPot(0);
    }

    public int GetPot()
    {
        return current_pot;
    }

    public void SetPot(int amt)
    {
        PV.RPC("RPC_SetPot", RpcTarget.All, amt);
    }

    [PunRPC]
    private void RPC_SetPot(int amt)
    {
        current_pot += amt;
        PotText.text = "Pot: " + GetPot().ToString();
    }

    public void AddToPot(int amt)
    {
        PV.RPC("RPC_AddToPot", RpcTarget.All, amt);
    }

    [PunRPC]
    private void RPC_AddToPot(int amt)
    {
        current_pot += amt;
        PotText.text = "Pot: " + GetPot().ToString();
    }
}
