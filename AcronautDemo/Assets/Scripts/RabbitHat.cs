using UnityEngine;
using System.Collections;

public class RabbitHat : MonoBehaviour {

	public RabbitHat matchPoint;
	PlayerController pc;

	[HideInInspector]
	public Transform exitPoint;
	[HideInInspector]
	public bool justTeleported = false;
	[HideInInspector]
	public float timer;
	[HideInInspector]
	public bool jumpedOut;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		exitPoint = matchPoint.gameObject.transform;
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
			pc.isTeleporting = true;
			matchPoint.justTeleported = true;
			pc.transform.position = exitPoint.position;

		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (justTeleported == true) {
			pc.isTeleporting = false;
			justTeleported = false;
			timer = 1f;
			jumpedOut = false;
		}
	}
}
