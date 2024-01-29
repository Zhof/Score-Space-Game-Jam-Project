using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator playerAnimator;

    public CharacterController2D characterController;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        
    }

    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
