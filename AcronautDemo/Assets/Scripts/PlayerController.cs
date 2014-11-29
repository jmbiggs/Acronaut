using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity;
	public float groundResistance; // slows horizontal velocity on ground
	public float airResistance; // slows horizontal velocity in air
	public float speed;
	public float jumpSpeed;
	public float dashLength;
	public float dashSpeed;
	public float horizAirDashSpeed;
	public float horizAirDashLength;
	public float vertAirDashLength;
	public float vertAirDashSpeed;

	public float wallSlideSpeed; // like a gravity value when wall sliding
	public float wallJumpSpeedVert;
	public float wallJumpSpeedHoriz;
	public float wallJumpLength; // amount of time to move horizontally in wall jump
	public float wallDashLength;
	public float hoverSpeed;
	public float knockbackDist;
	public float knockbackSpeed;
	public float swingPauseTime;
	public float swingSpeed;
	public float swingTrickSpeedMult;

	[HideInInspector]
	public float horizVelocity = 0f;
	[HideInInspector]
	public float horizTranslation = 0f;
	[HideInInspector]
	public float vertVelocity = 0f;
	[HideInInspector]
	public float gravityVelocity = 0f; // the current velocity due to gravity
	[HideInInspector]
	public float terminalVelocity = -3.3f; // the max speed the player can fall
	
	private PlayerPhysics pPhysics;
	[HideInInspector]
	public bool facingRight = true;

	private float dashTimer;
	private float wallJumpTimer;

	private float knockbackToTravel;
	private int knockbackDir;

	private bool swingTrick; // whether or not the trick button is held during swing
	public float swingPauseTimer;

	[HideInInspector]
	public bool isDashing = false;
	[HideInInspector]
	public bool isHorizAirDashing = false;
	[HideInInspector]
	public bool isVertAirDashing = false;
	[HideInInspector]
	public bool isWallDashing = false;
	[HideInInspector]
	public bool inWallJump = false;
	[HideInInspector]
	public bool isHovering = false;
	[HideInInspector]
	public bool isKnocked = false;
	[HideInInspector]
	public bool isSwinging = false;
	[HideInInspector]
	public bool inSpotlight = false;
	[HideInInspector]
	public bool paused = false;

	private bool hasUsedDoubleJump = false;
	private bool hasUsedHorizAirDash = false;
	private bool hasUsedVertAirDash = false;
	private bool hasUsedWallDash = false;

	private SpriteRenderer sprite;
	private Animator animator;

	public GameObject pausePanel;

	public void Jump(){
		vertVelocity += jumpSpeed;
	}

	public void KillJump(){
		vertVelocity = 0f;
	}

	public void Hover() {
		isHovering = true;
		vertVelocity = hoverSpeed;
		gravityVelocity = 0f;	
	}

	public void KillHover() {
		isHovering = false;
		gravityVelocity = 0f;	
	}

	public void AirMurder() {
		KillHover ();
		KillJump ();
		KillDash();
		KillHorizAirDash();
		KillVertAirDash();
	}

	public void WallJump(){
		inWallJump = true;
		vertVelocity = 0f;
		gravityVelocity = 0f;
		horizVelocity += wallJumpSpeedHoriz * -pPhysics.wallClingingDir;
		wallJumpTimer = wallJumpLength;
	}

	public void DoubleJump(){
		if (!inSpotlight)
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
		horizVelocity = dashSpeed;	
	}
	public void KillDash(){
		isDashing = false;
		horizVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// horizontal air dash in direction player is facing
	// uses same dash length and speed as ground dash
	public void HorizAirDash(int direction){
		if (!inSpotlight)
			hasUsedHorizAirDash = true;
		isHorizAirDashing = true;
		vertVelocity = 0f;
		gravityVelocity = 0f;
		dashTimer = horizAirDashLength;
		horizAirDashSpeed *= direction;
		horizVelocity = horizAirDashSpeed;
	}

	public void KillHorizAirDash(){
		isHorizAirDashing = false;
		horizVelocity = 0f;
		horizAirDashSpeed = Mathf.Abs(horizAirDashSpeed); // reset dash speed to its absolute value
	}

	// vertical air dash in given direction
	// -1 for down, 1 for up
	// uses same dash length and speed as ground dash

	// downward vertical dash feels weird to me when the timer runs out and you go back to a slower speed.
	// maybe animation will help this seem more normal, or maybe we shouldn't time downward dashes? -MB

	public void VertAirDash(int direction){
		if (!inSpotlight)
			hasUsedVertAirDash = true;
		isVertAirDashing = true;
		gravityVelocity = 0f;
		dashTimer = vertAirDashLength;
		vertAirDashSpeed *= direction;
		vertVelocity = vertAirDashSpeed;
	}

	public void KillVertAirDash(){
		isVertAirDashing = false;
		if (vertVelocity > 0f)
			vertVelocity = 0f;
		vertAirDashSpeed = Mathf.Abs(vertAirDashSpeed); // reset dash speed to its absolute value
	}

	// vertical wall dash in given direction
	// -1 for down, 1 for up
	// using same dash speed as the other dashes
	public void WallDash(int direction){
		if (!inSpotlight)
			hasUsedWallDash = true;
		isWallDashing = true;
		dashTimer = wallDashLength;
		dashSpeed *= direction;
		vertVelocity = dashSpeed;
	}
	public void KillWallDash(){
		isWallDashing = false;
		vertVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
	}

	// temporarily disables controls and knocks the player in the given direction
	// (-1 for left, 1 for right)
	public void Knockback(int direction) {
		if (isHorizAirDashing) {
			KillHorizAirDash();
		}
		else if (isDashing) {
			KillDash();
		}
		horizVelocity = 0f;
		isKnocked = true;
		knockbackDir = direction;
		knockbackToTravel = knockbackDist;
	}

	// temporarily disables controls and swings the player up into the air
	// if trick button is held down, swing distance will be longer
	public void Swing() {
		// isSwinging = true;
		// swingPauseTimer = swingPauseTime;
		// horizVelocity = 0f;
		// vertVelocity = 0f;
	}

	// restores ability to do all air moves
	public void RefreshAirMoves() {
		hasUsedDoubleJump = false;
		hasUsedHorizAirDash = false;
		hasUsedVertAirDash = false;
		hasUsedWallDash = false;
	}

	// called by PlayerPhysics when grounded
	public void SetGrounded(){
		if (pPhysics.grounded) {
			RefreshAirMoves();
			KillDash();
			if (isHovering) KillHover ();
			gravityVelocity = 0f;
			vertVelocity = 0f;
			horizVelocity = 0f;
		}
	}

	void Start() {
		pPhysics = GetComponent<PlayerPhysics> ();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.P)) {
			if (paused) {
				paused = false;
				Time.timeScale = 1f;
				pausePanel.SetActive(false);
			}
			else {
				paused = true;
				Time.timeScale = 0f;
				pausePanel.SetActive(true);
			}
		}

		if (paused)
			return;

		// apply gravity unless grounded, wall clinging, air dashing, swinging or in a wall jump
		if (!pPhysics.grounded && !pPhysics.wallClinging && !isHorizAirDashing && !isVertAirDashing && !inWallJump && !isHovering && !isSwinging) {
			if (vertVelocity >= terminalVelocity) {
				gravityVelocity += gravity * Time.deltaTime;
				vertVelocity -= gravityVelocity;
			}
			else if (vertVelocity < terminalVelocity) {
				vertVelocity -= terminalVelocity * Time.deltaTime;
				if (vertVelocity > terminalVelocity + .01f && vertVelocity < terminalVelocity - .01f)
					vertVelocity = terminalVelocity;
			}
		}
		// Apply reduced "gravity" if wall clinging		
		else if (pPhysics.wallClinging && !isWallDashing) {
			vertVelocity = wallSlideSpeed * -1;
		}

		// apply ground resistance if necessary
		if (pPhysics.grounded && horizVelocity != 0f && !isDashing && !isKnocked) {
			if (horizVelocity > 0){
				horizVelocity -= groundResistance * Time.deltaTime;
				if (horizVelocity < 0)
					horizVelocity = 0f;
			}
			else {
				horizVelocity += groundResistance * Time.deltaTime;
				if (horizVelocity > 0)
					horizVelocity = 0f;
			}
		}

		// apply air resistance if necessary
		else if (!pPhysics.grounded && horizVelocity != 0f && !isHorizAirDashing && !isVertAirDashing && !isKnocked) {
			if (horizVelocity > 0){
				horizVelocity -= airResistance * Time.deltaTime;
				if (horizVelocity < 0)
					horizVelocity = 0f;
			}
			else {
				horizVelocity += airResistance * Time.deltaTime;
				if (horizVelocity > 0)
					horizVelocity = 0f;
			}
		}

		// get the player's (possible) left/right input
		// it should be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal");
		
		// right direction
		if (horizInput > 0 && !isDashing && !isHorizAirDashing && !inWallJump && !isKnocked && !isSwinging) {

			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
			/*if (horizVelocity < 0)
				horizVelocity = 0f;*/
		}
		// left direction
		else if (horizInput < 0 && !isDashing && !isHorizAirDashing && !inWallJump && !isKnocked && !isSwinging) {
			animator.SetFloat("Speed", Mathf.Abs(horizInput));
			horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
			/*if (horizVelocity > 0)
				horizVelocity = 0f;*/
		}

		// get the player's (possible) up/down input
		// -1 for down, 1 for up
		var vertInput = Input.GetAxis ("Vertical");

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && !isKnocked && !isSwinging) {
			if (pPhysics.grounded) {
				Jump();
			}
			else if (pPhysics.wallClinging)
				WallJump();
			else if (!hasUsedDoubleJump) {
				DoubleJump();
			}
			else if (hasUsedDoubleJump) {
				Hover();
			}
		}
		if (Input.GetButtonUp ("Jump")) {
			if (isHovering) {
				KillHover ();
			}
			else
				KillJump();
		}



		// Handle the trick button

		// start ground dash
		if ((Input.GetButtonDown ("Trick")) && pPhysics.grounded && !isKnocked && !isSwinging) {
						Dash ();
				}


		// start horiz air dash
		else if ((Input.GetButtonDown ("Trick")) && vertInput == 0 && !pPhysics.grounded && !hasUsedHorizAirDash && !pPhysics.wallClinging && !isKnocked && !isSwinging) {
			if (isHovering) {
				KillHover ();
			}
			if (horizInput > 0)
				HorizAirDash (1);
			else
				HorizAirDash (-1);
		} 

		// handle trick button during swing
		else if ((Input.GetButtonDown ("Trick")) && isSwinging)
			swingTrick = true;

		if ((Input.GetButtonDown ("Trick")) && isSwinging)
			swingTrick = false;

		// cancel dash (canceled by using a Double Jump or a Vertical Airdash, if the player has not used those up)
		if (isHorizAirDashing) {
			// TODO

			// however, maybe we don't want to kill it by double jumping
			// the extra speed in a double jump (like on the ground) is fun
		}

		// start vert air dash
		if ((Input.GetButtonDown ("Trick")) && vertInput != 0 && !pPhysics.grounded && !hasUsedVertAirDash && !pPhysics.wallClinging && !isKnocked && !isSwinging) {
			if (isHovering) {
				KillHover ();
			}
			if (vertInput < 0)
				VertAirDash(-1);
			else
				VertAirDash(1);
		}
		// cancel dash (canceled by using a Horizontal Airdash or Double Jump)
		if (isVertAirDashing) {
			// TODO
		}

		// start wall dash
		if ((Input.GetButtonDown ("Trick")) && vertInput != 0 && !hasUsedWallDash && pPhysics.wallClinging && !isKnocked && !isSwinging) {
			if (vertInput < 0)
				WallDash(-1);
			else
				WallDash(1);
		}
		// cancel dash
		if (isWallDashing) {
			// TODO
		}


		// Update trick behavior based on time passed

		if (isKnocked) {
			float toTravel = knockbackSpeed * Time.deltaTime;
			knockbackToTravel -= toTravel;
			if (knockbackToTravel > 0)
			{
				horizTranslation = toTravel * knockbackDir;
			}
			else
			{
				isKnocked = false;
			}
		}

		else if (isSwinging) {
			// pause
			if (swingPauseTimer > 0){
				swingPauseTimer -= Time.deltaTime;
			}
			// launch
			else {
				isSwinging = false;
				gravityVelocity = 0f;
				vertVelocity = swingSpeed;
				if (swingTrick)
					vertVelocity *= swingTrickSpeedMult;
			}

		}


		else if (isDashing) {
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
				vertVelocity = wallJumpSpeedVert;
			}
		}
		else if (isWallDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillWallDash();
		}

		animator.SetFloat("Vertical Speed", (vertVelocity));
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