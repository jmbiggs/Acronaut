using UnityEngine;
using System.Collections;

public class Rhino : MonoBehaviour {

	private PlayerController pc;
	
	public float bounceSpeed;
	public float dashingBounceSpeed;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Bounce up, speed depends on whether player is dashing
	void OnTriggerEnter2D(Collider2D coll){
		float newSpeed = pc.isDashing ? dashingBounceSpeed : bounceSpeed;
		pc.vertVelocity = newSpeed;
		pc.gravityVelocity = 0f;
	}
}
