using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks {

    public static PhotonLobby lobby;

    public GameObject ConnectButton;
    public GameObject CancelButton;

    private void Awake()
    {
        lobby = this;
    }

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings();
	}

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
        ConnectButton.SetActive(true);
    }

    public void OnConnectButtonClicked() {
        Debug.Log("Connect button clicked.");
        ConnectButton.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random room but failed. There must be no rooms available.");
        CreateRoom();
    }

    void CreateRoom() {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a room but failed. There must be a room with the same name.");
        CreateRoom();
    }

    public void OnCancelButtonClicked() {
        Debug.Log("Cancel button clicked.");
        CancelButton.SetActive(false);
        ConnectButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
