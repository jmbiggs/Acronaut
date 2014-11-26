using UnityEngine;
using System.Collections;

public class Monkey : MonoBehaviour {
	private PlayerController pc;

	public float pauseTime = 0.25f;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		// move player to underneath

		pc.isSwinging = true;
		pc.swingPauseTimer = pauseTime;
		pc.horizVelocity = 0f;
		pc.vertVelocity = 0f;
	}
}
