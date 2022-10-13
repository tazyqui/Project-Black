using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    public float moveSpeed = 40f;

    private float horizontalMove = 0f;

    private bool jump = false;

    private bool crouch = false;

    public bool disabled = false;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
    }

    private void HandleMovement() 
    {
        if (!disabled)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
            
            HandleHorizontalAnimation(horizontalMove);

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;

                Debug.Log("crouch");
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }
        }
        else
        {
            horizontalMove = 0f;
        }
    }

    private void HandleHorizontalAnimation(float horizontalMove)
    {
        animator.SetFloat("Speed", MathF.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        //move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

}

