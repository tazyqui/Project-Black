using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public float moveSpeed = 40f;

    private float horizontalMove = 0f;

    private bool jump = false;

    private bool crouch = false;

    public bool disabled = false;

    // Update is called once per frame
    void Update()
    {
        if (!disabled) 
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

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
       


    }

    private void FixedUpdate()
    {
        //move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

}

