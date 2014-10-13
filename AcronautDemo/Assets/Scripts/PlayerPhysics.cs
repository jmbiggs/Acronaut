using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerPhysics : MonoBehaviour {

	public bool grounded;
	public int layerNumForCollisionMask;

	private int collisionMask;

	private BoxCollider2D coll;
	private Vector2 s;
	private Vector2 c;

	private int horizRays = 3;
	//private int vertRays = 3;
	private float rayBuffer = 0.005f;

	void Start() {
		coll = GetComponent<BoxCollider2D> ();
		collisionMask = 1 << layerNumForCollisionMask;

		// variables to make calculations easier to type
		s = coll.size;
		c = coll.center;
	}

	public void Move(float horizTranslation, float vertTranslation) {

		grounded = false;

		// direction to cast rays
		float dir = (vertTranslation > 0)? 1f : -1f;

		// coordinates of ray origin
		Vector2 p = transform.position;	
		float x = (p.x + c.x - s.x/2);
		float y = p.y + c.y + s.y/2 * dir;
		
		// length of ray
		float dist = (vertTranslation == 0)? 2 : Mathf.Abs (vertTranslation);

		// check for a collision
		RaycastHit2D hitInfo;

		for (int i=0; i < horizRays; i++) {
			
			Ray2D ray = new Ray2D(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay (ray.origin, ray.direction, Color.cyan);
			
			hitInfo = Physics2D.Raycast (ray.origin, ray.direction, dist, collisionMask);
			
			if (hitInfo.collider != null) {

				// distance between player and object
				float d = Vector2.Distance (ray.origin, hitInfo.point);

				if (d > rayBuffer){
					vertTranslation = -d + rayBuffer;
				}
				else {
					vertTranslation = 0;
				}

				grounded = true;
				break;
			}
			
			// adjust x coordinate for each iteration of the loop
			x += s.x/(horizRays - 1);
		}
		transform.Translate (horizTranslation, vertTranslation, 0);
	}	
}