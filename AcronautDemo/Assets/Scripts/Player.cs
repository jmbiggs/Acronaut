using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public bool grounded = true;
	public float jumpSpeed;
	public float jumpMaxHeight;
	public float hoverMax;
	public float hoverMultiplier;
	
	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpIsOver = true;  // whether or not the jump has run its course
	private bool jumpDescending = false;
	private float hoverCount; // TO ADD
	
	// ends any current jump
	public void KillJump() {
		jumpIsOver = true;
		jumpDescending = false;
	}
	
	// Update is called once per frame
	void Update () {

		// get the player's (possible) left/right input
		// it will be between -1 and 1
		var horizInput = Input.GetAxis ("Horizontal") * speed;		
		float translation = Time.deltaTime * speed;
		
		// right direction
		if (horizInput > 0) {
			transform.Translate (translation, 0, 0);
			transform.localScale = new Vector3(1, 1, 1); // face right
		}
		// left direction
		else if (horizInput < 0) {
			transform.Translate (-translation, 0, 0);
			transform.localScale = new Vector3(-1, 1, 1); // face left
		}
		
		
		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && grounded) {
			// initiate jump
			jumpOrigin = transform.position.y;
			jumpIsOver = false;
		}
		if (Input.GetButtonUp ("Jump"))
			// immediately stop jump
			jumpIsOver = true;
		if (Input.GetButton ("Jump") && !jumpIsOver) {
			float vertTranslation = Time.deltaTime * jumpSpeed;
			if (jumpDescending) {
				// descending
				hoverCount += Time.deltaTime;
				if (hoverCount < hoverMax){
					// slow down
					rigidbody2D.AddForce(new Vector2(0, hoverMultiplier));
				}
			}
			else if (transform.position.y <= (jumpMaxHeight + jumpOrigin)) {
				// ascending
				//print ("ascent");
				transform.Translate (0, vertTranslation, 0);
			} else {
				// start descent
				jumpDescending = true;
				hoverCount = 0;
			}
		}
	}
	
}