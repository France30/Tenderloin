using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonEvent : MonoBehaviour
{
    public void LoadGame()
    { 
        GameController.Instance.LoadGame();
    }

    public void QuitToMainMenu()
    {
        GameController.Instance.QuitToMainMenu();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void AddScoreToRanking(TMP_InputField nameInput)
    {
        ScoreManager.Instance.AddScore(new Score(name: nameInput.text , score: GameController.Instance.HighScore));
        GameController.Instance.GameScreenUI.GameOverRanking.SetActive(true);
    }
}
