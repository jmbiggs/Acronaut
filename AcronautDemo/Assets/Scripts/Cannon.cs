using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	
	private PlayerController pc;
	
	public float pauseTime;
	public float launchAngle;
	public float launchForce;

	private float timer;
	private bool inPause = false;
	private float hForce;
	private float vForce;

	private bool isFiring = false;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		if (inPause) {
			if (timer > 0){
				timer -= Time.deltaTime;
			}
			else {
				inPause = false;
				pc.paused = false;

				// launch
				float launchAngRad = launchAngle * Mathf.Deg2Rad;
				hForce = launchForce * Mathf.Cos (launchAngRad);
				vForce = launchForce * Mathf.Sin (launchAngRad);
				pc.horizVelocity = hForce;
				pc.vertVelocity = vForce;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (!isFiring) {
			pc.paused = true;
			pc.transform.position = this.gameObject.transform.position;
			inPause = true;
			timer = pauseTime;
			pc.horizVelocity = 0f;
			pc.vertVelocity = 0f;
			isFiring = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		isFiring = false;
	}

}
