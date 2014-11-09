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
	void OnCollisionEnter2D(Collision2D coll){
		float newSpeed = pc.isDashing ? dashingBounceSpeed : bounceSpeed;
		pc.vertVelocity = newSpeed;
	}
}
