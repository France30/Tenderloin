using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField] private float downedStateXOffset = 0.72f;
    [SerializeField] private float downedStateYOffset = 0.10f;

    private float rotationX, rotationY;

    private float baseCameraPosY;
    private float baseCameraPosX;
    private float downedCameraPosY;
    private float downedCameraPosX;
    

    [SerializeField]
    private float headRotationLimit = 60f;

    private bool isDownedOffsetSet = false;
    private PhotonView photonView;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerController player = target.GetComponent<PlayerController>();
        if (!player.PhotonView.IsMine)
        {
            this.GetComponent<Camera>().enabled = false;
            this.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            this.GetComponent<Camera>().enabled = true;
            this.GetComponent<AudioListener>().enabled = true;
        }

        photonView = target.GetComponent<PhotonView>(); 
        //Make sure the cursor stays in the center of the screen
        //Change the lock state of our cursor
        Cursor.lockState = CursorLockMode.Locked;

        baseCameraPosY = transform.localPosition.y;
        baseCameraPosX = transform.localPosition.x;

        downedCameraPosX = transform.localPosition.x + downedStateXOffset;
        downedCameraPosY = transform.localPosition.y + downedStateYOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!photonView.IsMine) return;

        bool gameOver = GameController.Instance.IsGameOver;
        if (gameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCursorLock();

        //look up and down is based on the x-axis rotation
        rotationX += Input.GetAxis("Mouse Y");
        //look left and right is based on the y-axis rotation
        rotationY += Input.GetAxis("Mouse X");

        //Limit the value of our lookup/down based on the head rotation value
        rotationX = Mathf.Clamp(rotationX, -headRotationLimit, headRotationLimit);
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;

        SetCameraPosition();

        //rotate the camera to face our mouse direction
        Quaternion rotation = Quaternion.Euler(-rotationX, rotationY, 0);
        transform.rotation = rotation;

        //rotate the target to face the camera direction
        target.transform.rotation = Quaternion.Euler(
            target.transform.rotation.x,
            rotationY,
            target.transform.rotation.z);
    }

    private void SetCameraPosition()
    {
        if (GameController.Instance.Player.IsPlayerDown && !isDownedOffsetSet)
        {
            isDownedOffsetSet = true;
            transform.localPosition = new Vector3(downedCameraPosX, downedCameraPosY, 0);
        }
        else if (!GameController.Instance.Player.IsPlayerDown && isDownedOffsetSet)
        {
            isDownedOffsetSet = false;
            transform.localPosition = new Vector3(baseCameraPosX, baseCameraPosY, 0);
        }
    }

    private void ToggleCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
