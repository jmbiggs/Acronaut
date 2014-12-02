using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	PlayerController pc;
	public float addedSpeedFromBelow = 10f;
	public float setSpeedFromAbove = 10f;
	public float timeToRespawn = 1.5f;
	public float timeToSubtract = 2f;
	private bool canEarnTime = true;

	private float respawnTimer = 0f;

	private Level level;

	private SpriteRenderer sprite;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		sprite = gameObject.GetComponent<SpriteRenderer>();
		box = gameObject.GetComponent<BoxCollider2D>();
		level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
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

			if (pc.isHorizAirDashing) {
				pc.KillHorizAirDash();
			}

			if (pc.isVertAirDashing) {
				pc.KillVertAirDash();
			}

			pc.vertVelocity = pc.jumpSpeed;
			pc.gravityVelocity = 0;

			pc.RefreshAirMoves();
			// Missing an animation for the balloon popping, but this will make the balloon disappear
			sprite.enabled = false;
			box.enabled = false;

			respawnTimer = timeToRespawn;

			if (Input.GetButton("Jump") || Input.GetButtonUp ("Jump")) {
				pc.killJumpOnButtonUp = false;
			}

			if (canEarnTime) {
				level.playerTime -= timeToSubtract;
				canEarnTime = false;
			}
		}

	}
}
