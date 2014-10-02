﻿using UnityEngine;
using System.Collections;

public class FootDetection : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		GameObject player = GameObject.FindWithTag ("Player");
		player.GetComponent<Player> ().grounded = true;
	}
	void OnTriggerExit2D (Collider2D other)
	{
		GameObject player = GameObject.FindWithTag ("Player");
		player.GetComponent<Player> ().grounded = false;
	}

}
