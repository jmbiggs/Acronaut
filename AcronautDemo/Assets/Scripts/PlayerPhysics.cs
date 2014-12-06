using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerController))]
public class PlayerPhysics : MonoBehaviour {

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool wallClinging;
	[HideInInspector]
	public float wallClingingDir;  // -1 for left cling, 1 for right cling

	public float raySizeNoVertVel; // downward raycast size when not moving vertically
	public int layerNumForCollisionMask;

	private int collisionMask;
	private BoxCollider2D coll;

	private bool wasGrounded;
	private bool wasClinging;

	// variables to make calculations easier to type
	private Vector2 s; // size of collider
	private Vector2 c; // center of collider

	private PlayerController pc;

	private int horizRays = 3;
	private int vertRays = 3;
	private float rayBuffer = 0.001f;

	void Start() {
		coll = GetComponent<BoxCollider2D> ();
		pc = GetComponent<PlayerController> ();
		collisionMask = 1 << layerNumForCollisionMask;

		s = coll.size;
		c = coll.center;
	}

	// Apply movement to the player while checking for collisions
	public void Move(float horizTranslation, float vertTranslation) {

		wasGrounded = grounded;
		grounded = false;
		wasClinging = wallClinging;
		wallClinging = false;

		// direction to cast rays
		float dir = (vertTranslation > 0)? 1f : -1f;

		// coordinates of ray origin
		Vector2 p = transform.position;
		float x = (p.x + c.x - s.x/2);
		float y = p.y + c.y + s.y/2 * dir;
		
		// length of ray
		// Also, let's only have rays appear when the player is moving downwards.
		// That way, if we decide to have a platform that the player can jump through from below,
		// They don't immediately snap to the platform once they jump through it.
		float dist = (vertTranslation == 0)? raySizeNoVertVel : Mathf.Abs (vertTranslation);

		RaycastHit2D hitInfo;

		// detect vertical collisions
		for (int i=0; i < horizRays; i++) {
			
			Ray2D ray = new Ray2D(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay (ray.origin, ray.direction*dist, Color.cyan);
			
			hitInfo = Physics2D.Raycast (ray.origin, ray.direction, dist, collisionMask);

			if (hitInfo.collider != null) {
				// distance between player and object
				float d = Vector2.Distance (ray.origin, hitInfo.point);

				// down
				// if detection below player, stop moving and set grounded
				if (dir == -1){
					if (d > rayBuffer){
						vertTranslation = -d + rayBuffer;
					}
					else {
						vertTranslation = 0;
					}

					grounded = true;
					if (!wasGrounded)
						pc.SetGrounded();
					break;
				}

				// up
				// if detection above player, stop moving and clear player's vertical velocity
				else{
					if (d > rayBuffer){
						vertTranslation = d - rayBuffer;
					}
					else {
						vertTranslation = 0;
					}
					pc.KillJump();
				}
			}

			// adjust x coordinate for each iteration of the loop
			x += s.x/(horizRays - 1);
		}

		// check for horizontal collisions
		if (horizTranslation != 0) {

			// direction to cast rays
			float dirH = (horizTranslation > 0)? 1f : -1f;

			// reset ray origin
			y = p.y + c.y - s.y/2;
			x = p.x + c.x + s.x/2 * dirH;

			// length of ray
			float distH = Mathf.Abs (horizTranslation);
			
			RaycastHit2D hitInfoH;
			
			// detect horiz collisions
			for (int i=0; i < vertRays; i++) {
				
				Ray2D rayH = new Ray2D(new Vector2(x,y), new Vector2(dirH, 0));
				Debug.DrawRay (rayH.origin, rayH.direction, Color.cyan);
				
				hitInfoH = Physics2D.Raycast (rayH.origin, rayH.direction, distH, collisionMask);
				
				if (hitInfoH.collider != null) {
					// distance between player and object
					float d = Vector2.Distance (rayH.origin, hitInfoH.point);

					// stop horizontal movement and set wall clinging
					if (d > rayBuffer){
						// left
						if (dirH == -1){
							horizTranslation = -d + rayBuffer;
						}
						// right
						else{
							horizTranslation = d - rayBuffer;
						}
					}
					else {
						horizTranslation = 0;
					}
					if (pc.vertVelocity < 0) {
						wallClinging = true;
						wallClingingDir = dirH;
						pc.RefreshAirMoves();
						if (!wasClinging)
							pc.SetGrounded();
						pc.horizVelocity = 0f;
						break;
					}
					else if (pc.vertVelocity >= 0) {
						pc.SetGrounded();
						break;
					}
				}

				// adjust y coordinate for each iteration of the loop
				y += s.y/(vertRays - 1);
			}

			if (grounded)
				wallClinging = false;
		}
		
		transform.Translate (horizTranslation, vertTranslation, 0);
	}	
}