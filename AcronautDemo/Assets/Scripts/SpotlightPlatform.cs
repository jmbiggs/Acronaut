using UnityEngine;
using System.Collections;

public class SpotlightPlatform : MonoBehaviour {
	
	private PlayerController pc;
	
	public float pauseTime; // amount of pause time on first contact (for animations, sound, etc to play)
	public float spotlightTime; // amount of time player will keep new abilities
	
	private bool used = false; // indicated whether this platform has already been used
	private bool isPaused = false;
	private bool inSpotlight = false;
	private float timer;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused) {
			if (timer > 0)
				timer -= Time.deltaTime;
			else { // begin spotlight mode!
				isPaused = false;
				inSpotlight = true;
				pc.paused = false;
				pc.inSpotlight = true;
				timer = spotlightTime;
			}
		}

		else if (inSpotlight) {
			if (timer > 0)
				timer -= Time.deltaTime;
			else { // end spotlight mode
				inSpotlight = false;
				pc.inSpotlight = false;
			}
		}
	}
	
	// Reverse the player's vertical velocity, and multiply it by the bounce multiplier
	void OnCollisionEnter2D(Collision2D coll){
		if (!used) {
			// start the pause timer
			isPaused = true;
			timer = pauseTime;

			// pause the player
			pc.paused = true;

			used = true;
		}
	}
}
