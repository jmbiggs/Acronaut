using UnityEngine;
using System.Collections;

public class Elephant_Water : MonoBehaviour {

	private PlayerController pc;

	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		print ("YOU HIT THE WATER!");
		if (pc.facingRight) {
			pc.Knockback (-1);
		} else {
			pc.Knockback(1);
		}
	}
}
