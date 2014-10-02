using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public Vector2 maxVelocity;
	public bool grounded;
	public float jumpSpeed;
	public float jumpMaxHeight;

	private float airSpeedMultiplier = 1f; //.3f;

	private float jumpOrigin;
	private bool jumpsOver = true;

	//private bool facingRight = true;


	public void KillJump() {
		jumpsOver = true;
	}

	// Update is called once per frame
	void Update () {
		var forceX = 0f;
		var forceY = 0f;

		var absVelX = Mathf.Abs (rigidbody2D.velocity.x);
		var absVelY = Mathf.Abs (rigidbody2D.velocity.y);

		// test whether player is airborne
		if (absVelY < .2f) {
			grounded = true;
		} else {
			grounded = false;
		}

		// get the player's (possible) left/right input
		// it will be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;

		if (absVelX < maxVelocity.x) {
			// right direction
			if (horizInput > 0){
				forceX = grounded ? speed : (speed * airSpeedMultiplier);
				transform.localScale = new Vector3(1, 1, 1);
			}
			// left direction
			else if (horizInput < 0){
				forceX = grounded ? -speed : (-speed * airSpeedMultiplier);
				transform.localScale = new Vector3(-1, 1, 1);
			}
		}

		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && grounded)
			jumpsOver = false;

		if (Input.GetButton ("Jump") && !jumpsOver) {
			if (grounded) {
				jumpOrigin = transform.position.y;
				jumpsOver = false;
			} 

			if (transform.position.y >= (jumpMaxHeight + jumpOrigin)) {
				forceY = 0;
				jumpsOver = true;
			} else {
				if (!jumpsOver && absVelY < maxVelocity.y)
				forceY = jumpSpeed;
			}
		}

		rigidbody2D.AddForce(new Vector2(forceX, forceY));
	}
}
