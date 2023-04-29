using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Realtime;
using Photon.Pun;

public class RoomInfoEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomName;
    [SerializeField]
    private TextMeshProUGUI playerInfo;
    [SerializeField]
    private Button joinRoom;

    public RoomInfo roomInfo;

    //Make sure that we also listen to the even whenever the listings is updated
    private void Start()
    {
       
    }

    private void OnDestroy()
    {
        
    }

    public void SetRoomInfo(RoomInfo info)
    {
        roomInfo = info;
        UpdateRoomInfo();
    }

    private void UpdateRoomInfo()
    {
        roomName.text = roomInfo.Name;
        playerInfo.text = string.Format("{0}/{1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        LobbyManager.Instance.ShowPanel("loading");
    }
}
