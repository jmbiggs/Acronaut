using UnityEngine;
using System.Collections;

public class RabbitHat : MonoBehaviour {

	public Transform exitPoint;
	public RabbitHat matchPoint;
	PlayerController pc;

	public bool justTeleported = false;

	public float timer;
	public bool jumpedOut;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		timer = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (justTeleported) {
			timer -= Time.deltaTime;
			if (timer < 0f && !jumpedOut) {
				pc.vertVelocity = 15f;
				pc.gravityVelocity = 0;
				pc.RefreshAirMoves();
				jumpedOut = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (!justTeleported) {
			matchPoint.justTeleported = true;
			pc.transform.position = exitPoint.position;

		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (justTeleported == true) {
			justTeleported = false;
			timer = 1f;
			jumpedOut = false;
		}
	}
}
