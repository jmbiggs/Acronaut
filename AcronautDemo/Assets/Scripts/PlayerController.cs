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
	public float wallJumpAngle;
	public float wallJumpSpeed;
	public float wallTrickJumpSpeed;
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
	public bool isHovering = false;
	[HideInInspector]
	public bool isKnocked = false;
	[HideInInspector]
	public bool isSwinging = false;
	[HideInInspector]
	public bool inSpotlight = false;
	[HideInInspector]
	public bool paused = false;
	[HideInInspector]
	public bool killJumpOnButtonUp = true;
	[HideInInspector]
	public bool isTeleporting = false;

	private bool hasUsedDoubleJump = false;
	private bool hasUsedHorizAirDash = false;
	private bool hasUsedVertAirDash = false;

	private SpriteRenderer sprite;
	private Animator animator;

	[HideInInspector]
	public GameObject pausePanel;

	public ParticleSystem left;
	public ParticleSystem right;
	public ParticleSystem up;
	public ParticleSystem down;
	public ParticleSystem hover;

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
		hover.enableEmission = true;
		hover.Play ();
	}

	public void KillHover() {
		isHovering = false;
		gravityVelocity = 0f;	
		hover.Stop();
		hover.enableEmission = false;
	}

	public void AirMurder() {
		KillHover ();
		KillJump ();
		KillDash();
		KillHorizAirDash();
		KillVertAirDash();
	}

	// if tricked is true, jump further out
	public void WallJump(bool tricked){
		gravityVelocity = 0f;
		if (tricked) {
			horizVelocity += wallTrickJumpSpeed * Mathf.Sin(wallJumpAngle*Mathf.Deg2Rad) * -pPhysics.wallClingingDir;
			vertVelocity += wallTrickJumpSpeed * Mathf.Cos(wallJumpAngle*Mathf.Deg2Rad);
		} 
		else {
			horizVelocity += 0.9f * wallJumpSpeed * Mathf.Sin(wallJumpAngle*Mathf.Deg2Rad) * -pPhysics.wallClingingDir;
			vertVelocity += wallJumpSpeed * Mathf.Cos(wallJumpAngle*Mathf.Deg2Rad);
		}
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
		if (!facingRight) {
			dashSpeed *= -1;
			right.Play();
		} else left.Play();
		horizVelocity = dashSpeed;	
		animator.SetBool("isHorizDashing", true);
	}

	public void KillDash(){
		isDashing = false;
		horizVelocity = 0f;
		dashSpeed = Mathf.Abs(dashSpeed); // reset dash speed to its absolute value
		animator.SetBool("isHorizDashing", false);
	}

	// horizontal air dash in direction player is facing
	// uses same dash length and speed as ground dash
	public void HorizAirDash(int direction){
		if (!inSpotlight)
			hasUsedHorizAirDash = true;
		if (isDashing)
			KillDash ();
		isHorizAirDashing = true;
		if (direction > 0) {
			left.Play();
		}
		else right.Play();
		vertVelocity = 0f;
		gravityVelocity = 0f;
		dashTimer = horizAirDashLength;
		horizAirDashSpeed *= direction;
		horizVelocity = horizAirDashSpeed;
		animator.SetBool("isHorizDashing", true);
	}

	public void KillHorizAirDash(){
		isHorizAirDashing = false;
		horizVelocity = 0f;
		horizAirDashSpeed = Mathf.Abs(horizAirDashSpeed); // reset dash speed to its absolute value
		dashTimer = 0f;
		animator.SetBool("isHorizDashing", false);
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
		if (direction > 0) 
			down.Play ();
		else
			up.Play ();
		gravityVelocity = 0f;
		dashTimer = vertAirDashLength;
		vertAirDashSpeed *= direction;
		vertVelocity = vertAirDashSpeed;
		animator.SetBool("isVertAirDashing", true);
	}

	public void KillVertAirDash(){
		isVertAirDashing = false;
		if (vertVelocity > 0f)
			vertVelocity = 0f;
		vertAirDashSpeed = Mathf.Abs(vertAirDashSpeed); // reset dash speed to its absolute value
		animator.SetBool("isVertAirDashing", false);
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
	}

	// called by PlayerPhysics when grounded
	public void SetGrounded(){
		//if (pPhysics.grounded) {
			RefreshAirMoves();
			if (isDashing) KillDash();
			if (isHovering) KillHover ();
			if (isHorizAirDashing) KillHorizAirDash();
			gravityVelocity = 0f;
			vertVelocity = 0f;
			horizVelocity = 0f;
		//}
	}

	void Start() {
		pPhysics = GetComponent<PlayerPhysics> ();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		pausePanel = GameObject.FindGameObjectWithTag("PausePanel");
	}

	void Update () {

		if (Input.GetButtonDown("Pause")) {
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

		// apply gravity unless grounded, wall clinging, air dashing, swinging
		if (!pPhysics.grounded && !pPhysics.wallClinging && !isHorizAirDashing && !isVertAirDashing && !isHovering && !isSwinging) {
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
		else if (pPhysics.wallClinging) {
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
		if (horizInput > 0 && !isHorizAirDashing 
		    && !isKnocked && !isSwinging) {
			if (!isDashing)
				if (horizVelocity < 0) // if horiz velocity is going in opposite direction, subtract from that
					horizVelocity += speed;
				else
					horizTranslation += horizInput * speed * Time.deltaTime;
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
		}
		// left direction
		else if (horizInput < 0 && !isHorizAirDashing 
		         && !isKnocked && !isSwinging) {
			if (!isDashing){
				if (horizVelocity > 0){ // if horiz velocity is going in opposite direction, subtract from that
					horizVelocity -= speed;
				}
				else
				horizTranslation += horizInput * speed * Time.deltaTime;
			}
			//animator.SetFloat("Speed", Mathf.Abs(horizInput));
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
		}

		// get the player's (possible) up/down input
		// -1 for down, 1 for up
		var vertInput = Input.GetAxis ("Vertical");

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && !isKnocked && !isTeleporting && !isSwinging) {

			if (pPhysics.grounded){
				Jump();
			}
			else if (pPhysics.wallClinging) {
				if (Input.GetButton("Trick"))
					WallJump(true); // tricked wall jump
				else
				    WallJump(false); // plain wall jump
			}
			else if (!hasUsedDoubleJump) {
				if (isHorizAirDashing)
					KillHorizAirDash();
				else if (isVertAirDashing)
					KillVertAirDash();
				DoubleJump();
			}
			else if (hasUsedDoubleJump) {
				Hover();
			}
		}
		if (Input.GetButtonUp ("Jump") && !isKnocked && !isTeleporting && !isSwinging) {
			if (isHovering) {
				KillHover ();
			}
			else if (isVertAirDashing) {
				gravityVelocity = 0f;
				vertVelocity = vertAirDashSpeed;
			}
			else if (!killJumpOnButtonUp) {
				killJumpOnButtonUp = true;
			}
			else
				KillJump();
		}



		// Handle the trick button

		// start ground dash
		if ((Input.GetButtonDown ("Trick")) && pPhysics.grounded && !isTeleporting && !isKnocked && !isSwinging) {
			Dash ();
		}


		// start horiz air dash
		else if ((Input.GetButtonDown ("Trick")) && vertInput == 0 && !pPhysics.grounded && !hasUsedHorizAirDash && !isTeleporting && !pPhysics.wallClinging && !isKnocked && !isSwinging) {
			if (isHovering)
				KillHover ();			
			else if (isVertAirDashing)
				KillVertAirDash();
			if (facingRight)
				HorizAirDash (1);
			else
				HorizAirDash (-1);
		} 

		// handle trick button during swing
		else if ((Input.GetButtonDown ("Trick")) && isSwinging)
			swingTrick = true;

		if ((Input.GetButtonDown ("Trick")) && isSwinging)
			swingTrick = false;

		// start vert air dash
		if ((Input.GetButtonDown ("Trick")) && vertInput != 0 && !pPhysics.grounded && !isTeleporting && !hasUsedVertAirDash && !pPhysics.wallClinging && !isKnocked && !isSwinging) {
			if (isHovering)
				KillHover ();
			else if (isHorizAirDashing)
				KillHorizAirDash();
			if (vertInput < 0)
				VertAirDash(-1);
			else
				VertAirDash(1);
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
			// flip around, keeping the dash going
			if ((horizInput > 0 && dashDir == -1) || (horizInput < 0 && dashDir == 1)){
				dashDir *= -1;
				dashSpeed *= -1;
				horizVelocity *= -1;
			}
			if (!Input.GetButton("Trick") || dashTimer <= 0)
				KillDash ();
			if (pPhysics.grounded)
				dashTimer -= Time.deltaTime;
		} 
		else if (isHorizAirDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0 || !Input.GetButton("Trick"))
				KillHorizAirDash ();
		} 
		else if (isVertAirDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer <= 0)
				KillVertAirDash ();
		} 

		animator.SetFloat("Vertical Speed", (vertVelocity));
		animator.SetFloat("Speed", Mathf.Abs(horizInput));
		animator.SetBool("Grounded", pPhysics.grounded);
		animator.SetBool("isClinging", pPhysics.wallClinging);
		animator.SetBool("isHorizDashing", isDashing);
		animator.SetBool("isHorizAirDashing", isHorizAirDashing);

		// call move
		pPhysics.Move (horizVelocity*Time.deltaTime + horizTranslation, vertVelocity*Time.deltaTime);

		// reset horizontal translation
		horizTranslation = 0f;
	}


	void Awake() {
		Application.targetFrameRate = 60;
	}
}