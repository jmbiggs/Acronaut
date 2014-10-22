using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity;
	public float speed;
	public float jumpSpeed;
	public float dashLength;
	public float dashSpeed;
	//	public float hoverMax;
	//	public float hoverMultiplier;

	public float horizVelocity = 0f;
	public float horizTranslation = 0f;
	public float vertVelocity = 0f;

	private PlayerPhysics pPhysics;
	private bool facingRight = true;

	private float gravityVelocity = 0f; // the current velocity due to gravity

	//private bool isJumping = false;
	private bool isDashing = false;
	private float dashTimer;
	
	//private float currentJump = 0f; // when in a jump
	//private float hoverCount; // TO ADD

	public void Jump(){
		vertVelocity += jumpSpeed;
	}
	public void KillJump(){
		vertVelocity -= jumpSpeed;
	}

	public void DoubleJump(){

	}

	// ground dash in direction player is facing
	public void Dash(){
		isDashing = true;
		dashTimer = dashLength;
		if (!facingRight)
			dashSpeed *= -1;
		horizVelocity += dashSpeed;
	}
	public void KillDash(){
		isDashing = false;
		horizVelocity -= dashSpeed;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// horizontal air dash in given direction
	// -1 for left, 1 for right
	public void HorizAirDash(int direction){

	}

	// vertical air dash in given direction
	// -1 for down, 1 for up
	public void VertAirDash(int direction){

	}

	// vertical wall dash in given direction
	// -1 for down, 1 for up
	public void WallDash(int direction){

	}

	void Start() {
		pPhysics = GetComponent<PlayerPhysics> ();
	}

	void Update () {

		// apply gravity
		if (!pPhysics.grounded) {
			gravityVelocity += gravity * Time.deltaTime;
			vertVelocity -= gravityVelocity;
		}
		
		if (pPhysics.grounded) {
			gravityVelocity = 0f;
			vertVelocity = 0f;
		}

		// get the player's (possible) left/right input
		// it should be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal");
		
		// right direction
		if (horizInput > 0 && !isDashing) {
			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
		}
		// left direction
		else if (horizInput < 0 && !isDashing) {
			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
		}

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && pPhysics.grounded) {
			Jump();
		}
		if (Input.GetButtonUp ("Jump")) {
			KillJump();
		}

		// Handle the trick button

		// start dash
		if ((Input.GetButtonDown ("Fire2")) && pPhysics.grounded) {
			Dash();
		}
		// cancel dash (if input is in opposite direction of dash)
		if (isDashing) {
			float dashDir = Mathf.Sign(dashSpeed);
			if ((horizInput > 0 && dashDir == -1) || (horizInput < 0 && dashDir == 1))
				KillDash();
		}
		
		
		// Update trick behavior based on time passed

		if (isDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillDash();
		}

		// call move
		pPhysics.Move (horizVelocity*Time.deltaTime + horizTranslation, vertVelocity*Time.deltaTime);

		// reset horizontal translation
		horizTranslation = 0f;
	}
}