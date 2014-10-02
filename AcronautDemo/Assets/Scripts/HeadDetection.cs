using UnityEngine;
using System.Collections;

public class HeadDetection : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		GameObject player = GameObject.FindWithTag ("Player");
		player.GetComponent<Player> ().KillJump ();

	}

}
