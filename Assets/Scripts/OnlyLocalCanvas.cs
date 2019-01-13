using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlyLocalCanvas : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            transform.Find("Canvas").gameObject.SetActive(true);
        }
        else transform.Find("Canvas").transform.Find("Private").gameObject.SetActive(false);
    }
}