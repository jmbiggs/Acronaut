using UnityEngine;
using System.Collections;

public class Tightrope : MonoBehaviour {
	
	private PlayerController pc;
	
	public float slowdownMultiplier;
	
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Slows the player down
	void OnCollisionEnter2D(Collision2D coll){
		pc.speed *= slowdownMultiplier;
	}

	// Returns player to normal speeds
	void OnCollisionExit2D(Collision2D coll){
		pc.speed /= slowdownMultiplier;
	}
}
