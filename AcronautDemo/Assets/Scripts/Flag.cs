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

	void OnTriggerEnter2D(Collider2D coll) {;
			level.Win();
		
	}
}
