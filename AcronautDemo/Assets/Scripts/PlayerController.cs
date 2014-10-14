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
	private float vertTranslation = 0f;

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

			if (currentJump < jumpMaxHeight)
				currentJump += jumpSpeed;
			else
				KillJump();

			vertTranslation += Time.deltaTime * currentJump;
		}

		// Handle trick button

		if (Input.GetButtonDown ("Fire2") && pPhysics.grounded) {
			// initiate dash
			dashTimer = dashLength;
			dashIsOver = false;
		}
		if (Input.GetButtonUp ("Fire2")) {
		}
		if (Input.GetButton ("Fire2") && !dashIsOver) {
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