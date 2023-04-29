using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField]
    private TextMeshProUGUI logger;

    public void Log(string message, string colorCode = null)
    {
        if (logger == null)
        {
            Debug.Log(message);
            return;
        }
        logger.text += (string.IsNullOrEmpty(colorCode) ?
            "" : "<color=" + colorCode + ">") +
            message + (string.IsNullOrEmpty(colorCode) ?
            "" : "</color>") + System.Environment.NewLine;
    }
}
