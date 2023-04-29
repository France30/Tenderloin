using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class NameInputField : MonoBehaviour
{
    private TMP_InputField input;
    [SerializeField] private Button connectButton;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
        connectButton.interactable = false;
    }

    public void SetPlayerNickName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            connectButton.interactable = false;
            return;
        }
        //Set the name of the player in photon
        PhotonNetwork.NickName = value;
        connectButton.interactable = true;
    }
}
