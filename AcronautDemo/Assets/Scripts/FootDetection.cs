using UnityEngine;
using System.Collections;

public class FootDetection : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter2D (Collider2D other)
	{
		player.GetComponent<Player> ().grounded = true;
		player.GetComponent<Player> ().KillJump ();
	}
	void OnTriggerExit2D (Collider2D other)
	{
		player.GetComponent<Player> ().grounded = false;
	}

}
