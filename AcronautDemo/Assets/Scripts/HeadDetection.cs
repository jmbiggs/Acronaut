using UnityEngine;
using System.Collections;

public class HeadDetection : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter2D (Collider2D other)
	{
		player.GetComponent<Player> ().KillJump ();

	}

}
