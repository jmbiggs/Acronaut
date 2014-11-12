using UnityEngine;
using System.Collections;

public class Hoop : MonoBehaviour {
	
	private PlayerController pc;
	
	// Use this for initialization
	void Start () {
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll){
		pc.Knockback (-1);	
	}
}
