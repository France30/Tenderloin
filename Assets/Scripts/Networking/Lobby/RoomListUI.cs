using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private RoomInfoEntry roomPrefab;
    [SerializeField]
    private Transform content;

    private List<RoomInfoEntry> listings = new List<RoomInfoEntry>();

    //Whenever the local client joins a room, clear all the room listings so we can properly refresh when the roomList is opened
    public override void OnJoinedRoom()
    {
        //Destroy each prefab in the listings
        foreach (RoomInfoEntry entry in listings)
            Destroy(entry.gameObject);
        //clear the list
        listings.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                //Handle removing the room information

                //Get the index of the room that has the same name as the current info
                int roomIndex = listings.FindIndex(r => r.roomInfo.Name == info.Name);

                //If we found a room with the same room name, delete the room from the listings
                if(roomIndex != -1)
                {
                    Destroy(listings[roomIndex].gameObject);
                    listings.RemoveAt(roomIndex);
                }
            }
            else
            {
                //Handle Adding/Updating room information
                //Check if the room is already existing in the roomList
                //Get the index of the room that has the same name as the current info
                int roomIndex = listings.FindIndex(r => r.roomInfo.Name == info.Name);

                //If we did not find a room with the same room name in the listings
                if (roomIndex == -1)
                {
                    //Add a roomlistprefab 
                    RoomInfoEntry item = Instantiate(roomPrefab, content);
                    item.SetRoomInfo(info);
                    listings.Add(item);
                }
                else
                {
                    //Update the room information if it already exists
                    listings[roomIndex].SetRoomInfo(info);
                }
            }
        }
    }
}
