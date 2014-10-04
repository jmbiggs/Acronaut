using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public Vector2 maxVelocity;
	public bool grounded = true;
	public float jumpSpeed;
	public float jumpMaxHeight;
	public float jumpHang;
	public float hoverMultiplier;
	
	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpIsOver = true;  // whether or not the jump has run its course
	private float hangCount = 0;
	private bool jumpDescending = false;
	private float hoverCount; // TO ADD
	
	// ends any current jump
	public void KillJump() {
		jumpIsOver = true;
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
			transform.localScale = new Vector3(1, 1, 1); // face right
		}
		// left direction
		else if (horizInput < 0) {
			transform.Translate (-translation, 0, 0);
			transform.localScale = new Vector3(-1, 1, 1); // face left
		}
		
		
		// Handle the jump button
		if (Input.GetButtonDown ("Jump") && grounded) {
			jumpOrigin = transform.position.y;
			jumpIsOver = false;
		}
		if (Input.GetButtonUp ("Jump"))
			jumpIsOver = true;
		if (Input.GetButton ("Jump") && !jumpIsOver) {
			float vertTranslation = Time.deltaTime * jumpSpeed;
			if (jumpDescending) {
				// descend
				//print ("descent");
				//vertTranslation *= hoverMultiplier;
				//transform.Translate (0, -vertTranslation, 0);
			}
			if (transform.position.y <= (jumpMaxHeight + jumpOrigin)) {
				// ascend
				print ("ascent");
				transform.Translate (0, vertTranslation, 0);
			} else {
				// hang
				print ("hang");
				hangCount += Time.deltaTime;
				if (hangCount > jumpHang)
					jumpDescending = true;
			}
		}
		
		// apply the new force to the player
		rigidbody2D.AddForce(new Vector2(0, forceY));
	}
	
}