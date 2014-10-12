using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public bool grounded;
	public float jumpSpeed;
	public float jumpMaxHeight;
	public float hoverMax;
	public float hoverMultiplier;
	public float gravity;

	private float horizTranslation;
	private float vertTranslation;

	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpIsOver = true;  // whether or not the jump has run its course
	//private bool falling = false;
	private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD

	// ends any current jump
	public void KillJump() {
		jumpIsOver = true;
		//falling = false;
	}
	
	// Update is called once per frame
	void Update () {

		// reset translations
		horizTranslation = 0f;
		vertTranslation = 0f;		
		
		// apply gravity (if necessary)
		if (!grounded) {
			vertTranslation -= gravity;
		}

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
		if (Input.GetButtonDown ("Jump") && grounded) {
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
				currentJump += Time.deltaTime * jumpSpeed;

			vertTranslation += currentJump;
		}

		transform.Translate (horizTranslation, vertTranslation, 0);

	}
	
}