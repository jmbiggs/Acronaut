using UnityEngine;
using System.Collections;

public class Rhino : MonoBehaviour {

	private PlayerController pc;
	
	public float bounceSpeed;
	public float dashingBounceSpeed;
	public float bounceAngle;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Bounce up, speed depends on whether player is dashing
	void OnTriggerEnter2D(Collider2D coll){

		// move player to the location we want
		pc.transform.position = this.gameObject.transform.position;
		
		float vertSpeed = pc.isDashing ? dashingBounceSpeed : bounceSpeed;
		float launchAngRad = bounceAngle * Mathf.Deg2Rad;
		float hForce = vertSpeed * Mathf.Cos (launchAngRad);
		float vForce = vertSpeed * Mathf.Sin (launchAngRad);
		pc.horizVelocity = hForce;
		pc.vertVelocity = vForce;
		pc.gravityVelocity = 0f;
	}
}
