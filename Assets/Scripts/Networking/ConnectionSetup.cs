using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadingPanel;
    private byte maxPlayersPerRoom = 2;
    private DebugManager debugger;

    private void Start()
    {
        debugger = DebugManager.Instance;
        loadingPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        debugger.Log("Connected to the master server.", "green");
        debugger.Log("Trying to join a random room");
        PhotonNetwork.JoinRandomRoom();
    }

    //When you joined a room
    public override void OnJoinedRoom()
    {
        debugger.Log("Successfully joined a room.", "green");
        DisplayRoomInformation();
        loadingPanel.SetActive(false);

        //Load the game scene here
        PhotonNetwork.LoadLevel(1);
    }

    //When another player enters your room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        debugger.Log(newPlayer.NickName + " has joined the room", "green");
        DisplayRoomInformation();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        debugger.Log(otherPlayer.NickName + " has exited the room", "red");
        DisplayRoomInformation();
    }

    private void DisplayRoomInformation()
    {
        debugger.Log(string.Format("{0}/{1} Players in {2}",
            PhotonNetwork.CurrentRoom.PlayerCount,
            PhotonNetwork.CurrentRoom.MaxPlayers,
            PhotonNetwork.CurrentRoom.Name), "orange");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        debugger.Log("Failed to join a random room: " + message, "red");
        debugger.Log("Try create a room instead.");
        //If no rooms are available (all rooms are at max capacity, no rooms created), we simply create a new room
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room",
            new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom });
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
            loadingPanel.SetActive(true);
        }
    } 
}
