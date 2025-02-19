using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float sprintSpeed = 10f;
    private float currentSpeed;
    private float gravity = -9.81f;

    [Header("Jump Settings")]
    public float jumpHeight = 1f;

    private bool isSprinting = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = speed; // Start with normal speed
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // Sprinting logic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        // Update speed based on sprinting state
        currentSpeed = isSprinting ? sprintSpeed : speed;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        playerVelocity.y += gravity * 1.5f * Time.deltaTime;

        if (controller.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
}
