using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	public PlayerController pc;
	public Level level;
	public BoxCollider2D box;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
			Debug.Log ("Collided!");
			level.Win();
		
	}
}
