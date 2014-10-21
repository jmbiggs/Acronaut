using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity;
	public float speed;

	public float jumpSpeed;
//	public float jumpMaxHeight;
	public float hoverMax;
	public float hoverMultiplier;

	public float dashLength;
	public float dashSpeed;
	
	public float horizTranslation = 0f;
	public float vertTranslation = 0f;

	private PlayerPhysics pPhysics;

	private bool facingRight = true;

	private float gravityVelocity = 0f; // the current velocity due to gravity

	private bool jumpIsOver = true;  // whether or not the jump has run its course
	//private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD


	private bool isDashing = false;
	private float dashTimer;

	public void KillJump(){
		jumpIsOver = true;
//		currentJump = 0f;
	}

	public void DoubleJump(){

	}

	// ground dash in given direction
	// -1 for left, 1 for right
	public void Dash(int direction){
		isDashing = true;
		dashTimer = dashLength;
		dashSpeed *= direction; // apply direction to dash speed
	}
	public void KillDash(){
		isDashing = false;
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
		/* Gravity needs to compound on itself the longer the player is in the air, since it's acceleration.
		 * Currently, gravity moves you down at a constant speed, but only once you let go of the jump button.
		 * Instead, lets have gravity ALWAYS affect the player when they're jumping, whether you're holding the jump button or not.
		 * This will give it a more natural look.
		 */
		if (!pPhysics.grounded) { // && jumpIsOver)
			gravityVelocity += gravity * Time.deltaTime;
			vertTranslation -= gravityVelocity;
		}
		if (pPhysics.grounded) {
			gravityVelocity = 0f;
		}

		// get the player's (possible) left/right input
		// it should be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;		
		horizTranslation += Time.deltaTime * horizInput;
		
		// right direction
		if (horizInput > 0 && !isDashing) {
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
		}
		// left direction
		else if (horizInput < 0 && !isDashing) {
			//horizTranslation *= -1;
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
		}

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && pPhysics.grounded) {
			// initiate jump
			//jumpOrigin = transform.position.y;
//			currentJump = 0f;
			jumpIsOver = false;
		}
		if (Input.GetButtonUp ("Jump")) {
			// immediately stop jump
			KillJump();
		}
		if (Input.GetButton ("Jump") && !jumpIsOver) {

			/* Lets remove jumpMaxHeight from this, I think it'll just cause problems later on.
			 * Instead, lets have the max height of a jump come organically from your jump speed
			 * and gravity.
			 */
			//if (currentJump < jumpMaxHeight)
			//	currentJump += jumpSpeed;
			//else
			//	KillJump();

			vertTranslation += Time.deltaTime * jumpSpeed;
		}
		if (Input.GetButton ("Jump") && pPhysics.grounded) {
			KillJump();
		}

		// Handle trick button

		// start dash
		if ((Input.GetButtonDown ("Fire2") || Input.GetKeyDown(KeyCode.Z)) && pPhysics.grounded) {
			if (horizInput > 0)
				Dash (1);
			else if (horizInput < 0)
				Dash (-1);
		}
		// cancel dash (if input is in opposite direction of dash)
		if (isDashing) {
			float dashDir = Mathf.Sign(dashSpeed);
			if ((horizInput > 0 && dashDir == -1) || (horizInput < 0 && dashDir == 1))
				KillDash();
		}
		
		
		// Apply tricks

		if (isDashing) {
			dashTimer -= Time.deltaTime;
			if (dashTimer > 0)
				horizTranslation += dashSpeed;
			else {
				KillDash();
			}
		}
		

		// call move
		pPhysics.Move (horizTranslation, vertTranslation);

		// reset translations
		horizTranslation = 0f;
		vertTranslation = 0f;

	}
}