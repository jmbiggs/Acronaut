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
	
	private float horizTranslation = 0f;
	private float vertTranslation = 0f;

	private PlayerPhysics pPhysics;

	//private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpIsOver = true;  // whether or not the jump has run its course
	private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD

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
		}
		// left direction
		else if (horizInput < 0) {
			//horizTranslation *= -1;
			transform.localScale = new Vector3(-1, 1, 1); // face left
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
			jumpIsOver = true;
			currentJump = 0f;
		}
		if (Input.GetButton ("Jump") && !jumpIsOver) {

			if (currentJump < jumpMaxHeight)
				currentJump += jumpSpeed;

			vertTranslation += Time.deltaTime * currentJump;
		}	

		pPhysics.Move (horizTranslation, vertTranslation);

		// reset translations
		horizTranslation = 0f;
		vertTranslation = 0f;


	}
}