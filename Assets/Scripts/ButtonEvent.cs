using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.RemoveRPCs(PhotonNetwork.MasterClient);
        else
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);

        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("Title Screen");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
