using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity;
	public float speed;
	public float jumpSpeed;
	public float dashLength;
	public float dashSpeed;
	public float vertAirDashLength;
	public float wallSlideSpeed; // like a gravity value when wall sliding
	public float wallJumpSpeed;
	public float wallJumpLength; // amount of time to move horizontally in wall jump

	[HideInInspector]
	public float horizVelocity = 0f;
	[HideInInspector]
	public float horizTranslation = 0f;
	[HideInInspector]
	public float vertVelocity = 0f;
	[HideInInspector]
	public float gravityVelocity = 0f; // the current velocity due to gravity
	[HideInInspector]
	public float terminalVelocity = -3f; // the max speed the player can fall
	
	private PlayerPhysics pPhysics;
	private bool facingRight = true;

	private float dashTimer;
	private float wallJumpTimer;

	private bool isDashing = false;
	private bool isHorizAirDashing = false;
	private bool isVertAirDashing = false;
	private bool inWallJump = false;

	private bool hasUsedDoubleJump = false;
	private bool hasUsedHorizAirDash = false;
	private bool hasUsedVertAirDash = false;

	private Animator animator;

	public void Jump(){
		vertVelocity += jumpSpeed;
	}
	public void KillJump(){
		vertVelocity = 0f;
	}

	public void WallJump(){
		inWallJump = true;
		vertVelocity = 0f;
		gravityVelocity = 0f;
		horizVelocity += wallJumpSpeed * -1 * pPhysics.wallClingingDir;
		wallJumpTimer = wallJumpLength;
	}

	public void DoubleJump(){
		hasUsedDoubleJump = true;
		vertVelocity = jumpSpeed;
		gravityVelocity = 0f;
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
		horizVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// horizontal air dash in direction player is facing
	// uses same dash length and speed as ground dash
	public void HorizAirDash(){
		hasUsedHorizAirDash = true;
		isHorizAirDashing = true;
		vertVelocity = 0f;
		gravityVelocity = 0f;
		dashTimer = dashLength;
		if (!facingRight)
			dashSpeed *= -1;
		horizVelocity += dashSpeed;
	}
	public void KillHorizAirDash(){
		isHorizAirDashing = false;
		horizVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// vertical air dash in given direction
	// -1 for down, 1 for up
	// uses same dash length and speed as ground dash
	public void VertAirDash(int direction){
		hasUsedVertAirDash = true;
		isVertAirDashing = true;
		gravityVelocity = 0f;
		dashTimer = vertAirDashLength;
		dashSpeed *= direction;
		vertVelocity = dashSpeed;
	}
	public void KillVertAirDash(){
		isVertAirDashing = false;
		vertVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// vertical wall dash in given direction
	// -1 for down, 1 for up
	public void WallDash(int direction){

	}

	// restores ability to do all air moves
	public void RefreshAirMoves() {
		hasUsedDoubleJump = false;
		hasUsedHorizAirDash = false;
		hasUsedVertAirDash = false;
	}

	// called by PlayerPhysics when grounded
	public void SetGrounded(){
		if (pPhysics.grounded) {
			RefreshAirMoves();
			gravityVelocity = 0f;
			vertVelocity = 0f;
			horizVelocity = 0f;
		}
	}

	void Start() {
		pPhysics = GetComponent<PlayerPhysics> ();
		animator = GetComponent<Animator>();
	}

	void Update () {

		// apply gravity unless grounded, wall clinging, air dashing or in a wall jump
		if (!pPhysics.grounded && !pPhysics.wallClinging && !isHorizAirDashing && !isVertAirDashing && !inWallJump) {
			if (vertVelocity >= terminalVelocity) {
				gravityVelocity += gravity * Time.deltaTime;
				vertVelocity -= gravityVelocity;
			}
		}
		// Apply reduced "gravity" if wall clinging		
		else if (pPhysics.wallClinging) {
			vertVelocity = wallSlideSpeed * -1;
		}

		// get the player's (possible) left/right input
		// it should be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal");
		
		// right direction
		if (horizInput > 0 && !isDashing && !inWallJump) {

			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
		}
		// left direction
		else if (horizInput < 0 && !isDashing && !inWallJump) {
			animator.SetFloat("Speed", Mathf.Abs(horizInput));
			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
		}

		// get the player's (possible) up/down input
		// -1 for down, 1 for up
		var vertInput = Input.GetAxis ("Vertical");

		// Handle the jump button
		if (Input.GetButtonDown ("Jump")) {
			if (pPhysics.grounded)
				Jump();
			else if (pPhysics.wallClinging)
				WallJump();
			else if (!hasUsedDoubleJump) {
				DoubleJump();
			}
		}
		if (Input.GetButtonUp ("Jump")) {
			KillJump();
		}

		// Handle the trick button

		// start ground dash
		if ((Input.GetButtonDown ("Trick")) && pPhysics.grounded) {
			Dash();
		}


		// start horiz air dash
		if ((Input.GetButtonDown ("Trick")) && vertInput == 0 && !pPhysics.grounded && !hasUsedHorizAirDash) {
			HorizAirDash();
		}
		// cancel dash (canceled by using a Double Jump or a Vertical Airdash, if the player has not used those up)
		if (isHorizAirDashing) {
			// TODO

			// however, maybe we don't want to kill it by double jumping
			// the extra speed in a double jump (like on the ground) is fun
		}

		// start vert air dash
		if ((Input.GetButtonDown ("Trick")) && vertInput != 0 && !pPhysics.grounded && !hasUsedVertAirDash) {
			if (vertInput < 0)
				VertAirDash(-1);
			else
				VertAirDash(1);
		}
		// cancel dash (canceled by using a Horizontal Airdash or Double Jump)
		if (isVertAirDashing) {
			// TODO
		}



		// Update trick behavior based on time passed

		if (isDashing) {
			float dashDir = Mathf.Sign (dashSpeed);
			if ((horizInput > 0 && dashDir == -1) || (horizInput < 0 && dashDir == 1))
				KillDash ();
			if (pPhysics.grounded)
				dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillDash ();
		} 
		else if (isHorizAirDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillHorizAirDash ();
		} 
		else if (isVertAirDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillVertAirDash ();
		} 
		else if (inWallJump) {
			wallJumpTimer -= Time.deltaTime;
			if (wallJumpTimer <= 0) {
				inWallJump = false;
				horizVelocity = 0f;
				vertVelocity = wallJumpSpeed;
			}
		}

		animator.SetFloat("Speed", Mathf.Abs(horizInput));
		animator.SetBool("Grounded", pPhysics.grounded);

		// call move
		pPhysics.Move (horizVelocity*Time.deltaTime + horizTranslation, vertVelocity*Time.deltaTime);

		// reset horizontal translation
		horizTranslation = 0f;
	}


	void Awake() {
		Application.targetFrameRate = 60;
	}
}