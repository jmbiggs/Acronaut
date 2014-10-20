using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	public float gravity;
	public float speed;

	public float jumpSpeed;
	public float jumpMaxHeight;
	public float hoverMax;
	public float hoverMultiplier;

	public float dashLength;
	public float dashMultiplier;
	
	private float horizTranslation = 0f;
	public float vertTranslation = 0f;

	private PlayerPhysics pPhysics;

	private bool facingRight = true;

	private bool jumpIsOver = true;  // whether or not the jump has run its course
	private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD

	private float dashTimer;
	private bool dashIsOver = false;

	public void KillJump(){
		jumpIsOver = true;
		currentJump = 0f;
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
		if (!pPhysics.grounded && jumpIsOver)
			vertTranslation -= gravity * Time.deltaTime;

		// get the player's (possible) left/right input
		// it will be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;		
		horizTranslation += Time.deltaTime * horizInput;
		
		// right direction
		if (horizInput > 0) {
			transform.localScale = new Vector3(1, 1, 1); // face right
			facingRight = true;
		}
		// left direction
		else if (horizInput < 0) {
			//horizTranslation *= -1;
			transform.localScale = new Vector3(-1, 1, 1); // face left
			facingRight = false;
		}

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && pPhysics.grounded) {
			// initiate jump
			//jumpOrigin = transform.position.y;
			currentJump = 0f;
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
			if (currentJump < jumpMaxHeight)
				currentJump += jumpSpeed;
			else
				KillJump();

			vertTranslation += Time.deltaTime * currentJump;
		}

		// Handle trick button

		if ((Input.GetButtonDown ("Fire2") || Input.GetKeyDown(KeyCode.Z)) && pPhysics.grounded) {
			// initiate dash
			dashTimer = dashLength;
			dashIsOver = false;
		}
		if (Input.GetButtonUp ("Fire2")) {
		}
		if ((Input.GetButton ("Fire2") || Input.GetKey(KeyCode.X)) && !dashIsOver) {
			dashTimer -= Time.deltaTime;
			if (dashTimer > 0)
				horizTranslation *= dashMultiplier;
			else
				dashIsOver = true;
		}



		pPhysics.Move (horizTranslation, vertTranslation);

		// reset translations
		horizTranslation = 0f;
		vertTranslation = 0f;


	}
}