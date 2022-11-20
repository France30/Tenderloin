using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicatorTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GameController.Instance.GameScreenUI.UpdateDownedIndicator(true, this.transform);
    }
}
