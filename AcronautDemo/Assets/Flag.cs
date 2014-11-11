using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	PlayerController pc;
	Level level;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			level.Win();
		}
		
	}
}
