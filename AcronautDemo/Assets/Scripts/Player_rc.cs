using UnityEngine;
using System.Collections;

public class Player_rc : MonoBehaviour {

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
	private bool falling = false;
	private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD

	private int horizRays = 6;
	private int vertRays = 4;
	private int margin = 2;

	// ends any current jump
	public void KillJump() {
		jumpIsOver = true;
		falling = false;
	}
	
	// Update is called once per frame
	void Update () {

		Rect box = new Rect (
			collider.bounds.min.x,
			collider.bounds.min.y,
			collider.bounds.size.x,
			collider.bounds.size.y
		);

		// reset translations
		horizTranslation = 0f;
		vertTranslation = 0f;

		// detect collisions
		float dist = 0.6f;
		RaycastHit hitInfo;
		Vector3 startPt = new Vector3 (box.xMin + margin, box.center.y, transform.position.z);
		Vector3 endPt = new Vector3 (box.xMax - margin, box.center.y, transform.position.z);

		bool hit = false;

		for (int i=0; i < vertRays; i++) {		
			float lerpAmount = (float)i / (float)vertRays - 1;
			Vector3 origin = Vector3.Lerp(startPt, endPt, lerpAmount);
			Ray ray = new Ray(origin, Vector3.down);

			hit = Physics.Raycast(ray, out hitInfo, dist);

			if (hit){
				grounded = true;
				falling = false;
				break;
			}
		}
		if (!hit)
			grounded = false;

		// apply gravity (if necessary)
		if (!grounded) {
			vertTranslation += Time.deltaTime - gravity;
			if (vertTranslation < 0)
				falling = true;
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
			jumpOrigin = transform.position.y;
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

		transform.Translate (horizTranslation, vertTranslation, 0);

	}
	
}