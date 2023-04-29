using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : SingletonPun<LobbyManager>
{
    [SerializeField] private GameObject connectionPanel, loadingPanel, roomListPanel, playerListPanel;
    private byte maxPlayersPerRoom = 4;
    private DebugManager debugger;

    private void Start()
    {
        debugger = DebugManager.Instance;
        ShowPanel("connection");
    }

    public override void OnConnectedToMaster()
    {
        debugger.Log("Connected to the master server.", "green");
        debugger.Log("Trying to join default lobby.");
        //Join the lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        debugger.Log("Successfully joined lobby.", "green");
        ShowPanel("roomList");
    }

    public override void OnCreatedRoom()
    {
        debugger.Log("Room successfully created!", "green");
        ShowPanel("playerList");
    }

    public override void OnJoinedRoom()
    {
        ShowPanel("playerList");
    }
    public override void OnLeftRoom()
    {
        ShowPanel("roomList");
    }

    public void Connect()
    {
        // Check first if we are already connected to the Photon Network
        if (PhotonNetwork.IsConnected)
        {
            debugger.Log("Already connected to photon network.");
        }
        else
        {
            debugger.Log("Not yet connected to photon network. Trying to connect..");
            PhotonNetwork.ConnectUsingSettings();
            ShowPanel("loading");
        }
    }

    public void ShowPanel(string panelName)
    {
        loadingPanel.SetActive(panelName.Equals(loadingPanel.name));
        connectionPanel.SetActive(panelName.Equals(connectionPanel.name));
        roomListPanel.SetActive(panelName.Equals(roomListPanel.name));
        playerListPanel.SetActive(panelName.Equals(playerListPanel.name));
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        options.IsVisible = true;
        //options.IsOpen = true;
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "'s Room", options);
        debugger.Log("Trying to create a room...");
        ShowPanel("loading");
    }

    public void LeaveRoom() 
    {
        debugger.Log("Trying to leave the room...");
        ShowPanel("loading");
        PhotonNetwork.LeaveRoom();
    }
}
