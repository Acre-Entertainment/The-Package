using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public Animator animator;
	public float runSpeed = 40f;
	float horizontalMove = 0f;
    float verticalMove = 0f;
	//Criar o float the movimento vertical
	bool jump = false;
	bool crouch = false;
	public bool isSwinging;
	
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical");
		//Aplicar

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		//animator.SetFloat("Vertical Speed", Mathf.Abs(verticalMove));
		//Botar no animator

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
			animator.SetBool("IsCrouching", true);
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
			animator.SetBool("IsCrouching", false);
		}

	
	}

	public void OnLanding()
		{
			animator.SetBool("IsJumping", false);
		}


	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
