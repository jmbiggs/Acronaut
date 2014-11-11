using UnityEngine;
using System.Collections;

public class Bounce_Platform : MonoBehaviour {

	private PlayerController pc;

	public float bounceMultiplier;
	
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	// Reverse the player's vertical velocity, and multiply it by the bounce multiplier
	void OnCollisionEnter2D(Collision2D coll){
		pc.RefreshAirMoves();
		pc.gravityVelocity = 0f;
		pc.vertVelocity *= bounceMultiplier * -1;
	}
}
