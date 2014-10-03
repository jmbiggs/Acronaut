using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public Vector2 maxVelocity;
	public bool grounded;
	public float jumpSpeed;
	public float jumpMaxHeight;
	
	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpsOver = true;  // whether or not the jump has run its course
	
	// ends any current jump
	public void KillJump() {
		jumpsOver = true;
	}
	
	// Update is called once per frame
	void Update () {
		// forces to be added to player's motion
		var forceY = 0f;
		
		
		// get the player's (possible) left/right input
		// it will be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;
		
		float translation = Time.deltaTime * speed;
		
		// right direction
		if (horizInput > 0) {
			transform.Translate (translation, 0, 0);
			transform.localScale = new Vector3(1, 1, 1);
		}
		// left direction
		else if (horizInput < 0) {
			transform.Translate (-translation, 0, 0);
			transform.localScale = new Vector3(-1, 1, 1);
		}
		
		
		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && grounded) {
			jumpOrigin = transform.position.y;
			jumpsOver = false;
		}
		if (Input.GetButtonUp ("Jump"))
			jumpsOver = true;
		if (Input.GetButton ("Jump") && !jumpsOver) {
			if (transform.position.y >= (jumpMaxHeight + jumpOrigin)) {
				forceY = 0f;
				jumpsOver = true;
			} else {
				forceY = jumpSpeed;
			}
		}
		
		// apply the new force to the player
		rigidbody2D.AddForce(new Vector2(0, forceY));
	}
	
}