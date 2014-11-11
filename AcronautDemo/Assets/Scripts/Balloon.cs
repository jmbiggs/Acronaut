using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public PlayerController pc;
	public float addedSpeedFromBelow = 10f;
	public float setSpeedFromAbove = 10f;
	public float timeToRespawn = 1.5f;

	private float respawnTimer = 0f;

	private SpriteRenderer sprite;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		sprite = gameObject.GetComponent<SpriteRenderer>();
		box = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// This will control the balloon respawning after a set amount of time
		if (respawnTimer > 0f) {
			respawnTimer -= Time.deltaTime;
			if (respawnTimer <= 0) {
				respawnTimer = 0f;
				sprite.enabled = true;
				box.enabled = true;
			}
		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			if (pc.isHovering) {
				pc.KillHover();
			}

			pc.vertVelocity = pc.jumpSpeed;
			pc.gravityVelocity = 0;

			pc.RefreshAirMoves();
			// Missing an animation for the balloon popping, but this will make the balloon disappear
			sprite.enabled = false;
			box.enabled = false;

			respawnTimer = timeToRespawn;
		}

	}
}
