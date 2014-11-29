using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {
	
	private PlayerController pc;
	private CircleCollider2D circle;
	
	public float bounceMultiplier;
	public float minBounce;
	
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		circle = gameObject.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Reverse the player's vertical and horizontal velocities
	void OnCollisionEnter2D(Collision2D coll){

		pc.RefreshAirMoves();

		// get middle contact point
		int mid = coll.contacts.Length / 2;
		Vector2 c = coll.contacts [mid].point;

		// get the normal
		Vector2 norm = coll.contacts [mid].normal;

		/*
		// get weighted percentages of contact point's distance from center
		float vPer = (c.y - transform.position.y); // / circle.radius;
		float hPer = (c.x - transform.position.x); // / circle.radius;
		*/

		// get magnitude of current velocity
		float magSquared = (pc.horizVelocity * pc.horizVelocity) + (pc.vertVelocity * pc.vertVelocity);
		float mag = Mathf.Sqrt (magSquared);

		// make sure it is at least the minimum
		if (mag < minBounce)
			mag = minBounce;

		// multiply each percentage by the magnitude, and set new velocities
		pc.vertVelocity = -norm.y * mag;
		pc.horizVelocity = -norm.x * mag;	

		// kill hover if necessary
		if (pc.isHovering)
			pc.KillHover ();
	}
}
