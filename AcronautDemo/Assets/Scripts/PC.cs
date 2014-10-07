﻿using UnityEngine;
using System.Collections;

public class PC : MonoBehaviour {
	
	public float speed;
	public Vector2 maxVelocity;
	public bool grounded;
	public float jumpSpeed;
	public float jumpMaxHeight;
	public float gravity;
	public float currentJump;


	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpsOver = true;  // whether or not the jump has run its course
	public bool hasDoubleJumped = false;

	// ends any current jump
	public void KillJump() {
		jumpsOver = true;
	}
	
	// Update is called once per frame
	void Update () {
		// forces to be added to player's motion
		var forceX = 0f;
		var forceY = 0f;
		
		
		// get the player's (possible) left/right input
		// it will be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;

		float translation = Time.deltaTime * speed;
		
		// right direction
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position = new Vector3(transform.position.x + (Time.deltaTime * speed), transform.position.y, transform.position.z);
			transform.localScale = new Vector3(1, 1, 1);
		}
		// left direction
		else if (Input.GetKey(KeyCode.LeftArrow)) {		
			transform.position = new Vector3(transform.position.x - (Time.deltaTime * speed), transform.position.y, transform.position.z);
			transform.localScale = new Vector3(-1, 1, 1);
		}
		


		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && grounded) {
			// grounded = false;
			currentJump = jumpSpeed;
		}
		if (Input.GetButtonUp ("Jump"))
			currentJump = 0f;
			jumpsOver = true;
		if (Input.GetButtonDown ("Jump") && jumpsOver && !hasDoubleJumped) {
			currentJump = jumpSpeed;
			hasDoubleJumped = true;
		}
		if (currentJump > -50f) {
			currentJump -= gravity;
		}
		if (!grounded)
			transform.position = new Vector3(transform.position.x, transform.position.y + (currentJump * Time.deltaTime), transform.position.z);
		
		// apply the new force to the player
		// rigidbody2D.AddForce(new Vector2(forceX, forceY));
	}

}