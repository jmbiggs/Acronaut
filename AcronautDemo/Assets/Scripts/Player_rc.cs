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

	private float horizTranslation = 0f;
	private float vertTranslation = 0f;

	private BoxCollider coll;

	private float jumpOrigin; // the y position of player when a jump first begins
	private bool jumpIsOver = true;  // whether or not the jump has run its course
	private bool falling = false;
	private float currentJump = 0f; // when in a jump
	private float hoverCount; // TO ADD

	private float margin = .05f;

	private int layerMask;

	// ends any current jump
	public void KillJump() {
		jumpIsOver = true;
		falling = false;
	}

	void Start() {
		layerMask = LayerMask.NameToLayer ("normalCollisions");
		coll = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {

		Rect box = new Rect (
			collider.bounds.min.x,
			collider.bounds.max.y,
			collider.bounds.size.x,
			collider.bounds.size.y
		);

		// reset translations
		//horizTranslation = 0f;
		//vertTranslation = 0f;

		// apply gravity (if necessary)
		if (!grounded) {
			vertTranslation -= gravity;
			if (vertTranslation < 0)
				falling = true;
		}

		if (grounded || falling) {

			// detect collisions
			float dist = box.height / 2 + (grounded ? margin : Mathf.Abs (vertTranslation * Time.deltaTime));
			RaycastHit hitInfo;

			Vector2 p = transform.position;
			Vector3 s = coll.size;
			Vector3 c = coll.center;
			float dir = Mathf.Sign (vertTranslation);

			/*
			Vector3 startPt = new Vector3 (box.xMin + margin, box.center.y, transform.position.z);
			Vector3 endPt = new Vector3 (box.xMax - margin, box.center.y, transform.position.z);
*/
			bool hit = false;

			for (int i=0; i < 3; i++) {

				float x = (p.x + c.x - s.x/2) + s.x/2 * i;
				float y = p.y + c.y + s.y/2 * dir;

				Ray ray = new Ray(new Vector3(x,y,0), new Vector3(0,dir,0));

				/*
					float lerpAmount = (float)i / (float)vertRays - 1;
					Vector3 origin = Vector3.Lerp (startPt, endPt, lerpAmount);
					Ray ray = new Ray (origin, Vector3.down);
				*/
				hit = Physics.Raycast (ray, out hitInfo, dist, layerMask);

				Debug.DrawRay (new Vector3(x,y,0), Vector3.down, Color.cyan);

				if (hit) {
					grounded = true;
					falling = false;
					vertTranslation = 0;
					break;
				}
			}
			if (!hit)
				grounded = false;
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