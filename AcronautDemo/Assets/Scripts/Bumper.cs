using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {
	
	private PlayerController pc;
	
	public float bounceMultiplier;
	
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Reverse the player's vertical and horizontal velocities
	void OnCollisionEnter2D(Collision2D coll){
		pc.RefreshAirMoves();
		pc.vertVelocity *= bounceMultiplier * -1;
		pc.horizVelocity *= bounceMultiplier * -1;
	}
}
