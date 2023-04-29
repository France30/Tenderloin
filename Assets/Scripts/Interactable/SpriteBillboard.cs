using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool freezeXZAxis = true;

    private void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        try
        {
            if (freezeXZAxis)
                transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
            else
                transform.rotation = Camera.main.transform.rotation;
        }
        catch
        {
            //do nothing
        }
    }
}
