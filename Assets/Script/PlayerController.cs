using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [Range(0.1f,10)]
    [SerializeField] private float gravoty;
    [SerializeField] private float jumpDistance;
    
    private PlayerInput playerInput;
    private Rigidbody rigidbody;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    

    private void Start()
    {
        playerInput = new PlayerInput();
        rigidbody = GetComponent<Rigidbody>();
        playerInput.Enable();
        playerInput.Player.Jump.performed += context => Jump();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Debug.Log(controller.isGrounded);
    }

    void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            var move = playerInput.Player.Move.ReadValue<Vector2>();
            Debug.Log($"Move {move}");
            moveDirection = new Vector3(move.x,0,move.y);
            moveDirection *= speed;
        }
        
        moveDirection.y -= gravoty;
        controller.Move(moveDirection);
    }
    
    private void Jump()
    {
        if (controller.isGrounded)
        {
            moveDirection.y += jumpDistance;
        }
        controller.Move(moveDirection);
    }

    // void FixedUpdate()
    // {
    //     var walk = playerInput.Player.Move.ReadValue<Vector2>();
    //     rigidbody.velocity = walk.normalized * speed;
    // }
}
