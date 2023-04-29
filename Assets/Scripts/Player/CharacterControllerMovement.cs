using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterControllerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 4.0f;

    [Range(0, 1)]
    [SerializeField] private float downedSpeed = 0.25f;

    [SerializeField] private float jumpHeight = 5.0f;
    [SerializeField] private float gravityScale = 1.0f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private float baseSpeed;
    private float gravity = -9.8f;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float distToGround = 0.4f;

    private float speedScale = 1f;

    private CharacterController characterController;
    private PlayerController player;

    private PhotonView photonView;

    public float MoveSpeed 
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;

            if (moveSpeed <= 0) moveSpeed = 1;
            baseSpeed = moveSpeed;
        }
    }

    public float SprintSpeed
    {
        get { return sprintSpeed; }
        set
        {
            sprintSpeed = value;

            if (sprintSpeed <= 0) sprintSpeed = 2;
        }
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();
        player = GetComponent<PlayerController>();

        baseSpeed = moveSpeed;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!player.PhotonView.IsMine) return;

        SetSpeedScale();

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.right * xMove) + (transform.forward * zMove);
        moveDirection *= moveSpeed * Time.deltaTime;
        //Debug.Log(moveDirection);
        characterController.Move(moveDirection * speedScale);

        ApplyGravity();

        if (player.IsPlayerDown) return;

        Walk(xMove,zMove);
        Sprint();
        Jump();       
    }

    private void Walk(float xMove, float zMove)
    {
        if (!isGrounded || player.IsPlayerSprinting) return;

        if (zMove < 0)
            photonView.RPC("RPCPlayAnimation", RpcTarget.All, "Walk Backward");
        else if (xMove != 0 || zMove > 0)
            photonView.RPC("RPCPlayAnimation", RpcTarget.All, "Walk");
        else
            photonView.RPC("RPCPlayAnimation", RpcTarget.All, "Idle");
    }

    private void Sprint()
    {
        bool sprintKeyPress = (Input.GetKey("left shift") || Input.GetKey("right shift"));
        if (sprintKeyPress && isGrounded && !player.IsPlayerFiring && !player.IsPlayerReloading)
        {
            player.IsPlayerSprinting = true;
            moveSpeed = sprintSpeed;
            photonView.RPC("RPCPlayAnimation", RpcTarget.All, "Run");
        }
        else
        {
            player.IsPlayerSprinting = false;
            moveSpeed = baseSpeed;
            photonView.RPC("RPCStopAnimation", RpcTarget.All, "Run");
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            photonView.RPC("RPCPlayAnimation", RpcTarget.All, "Jump");
        }
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, distToGround, groundMask);
        if (isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime *gravityScale;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void SetSpeedScale()
    {
        if (GameController.Instance.Player.IsPlayerDown)
            speedScale = downedSpeed;
        else
            speedScale = 1f;
    }

    [PunRPC]
    private void RPCPlayAnimation(string animation)
    {
        player.PlayerAnimation.Play(animation);
    }

    [PunRPC]
    private void RPCStopAnimation(string animation)
    {
        player.PlayerAnimation.Stop(animation);
    }
}
