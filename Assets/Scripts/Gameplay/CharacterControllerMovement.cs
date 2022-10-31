using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 5.0f;
    [SerializeField]
    private float gravityScale = 1.0f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;

    private float gravity = -9.8f;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float distToGround = 0.4f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.right * xMove) + (transform.forward * zMove);
        moveDirection *= moveSpeed * Time.deltaTime;
        //Debug.Log(moveDirection);
        characterController.Move(moveDirection);

        Jump();
        ApplyGravity();
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, distToGround, groundMask);
        if (isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime *gravityScale;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
