using UnityEngine;
using System.Collections;

public class SideDetection : MonoBehaviour {

	public GameObject player;

	void OnTriggerEnter2D (Collider2D other)
	{
		//print ("in");
		//player.GetComponent<Player> ().grounded = true;
	}
	void OnTriggerExit2D (Collider2D other)
	{
		//print ("out");
		//player.GetComponent<Player> ().grounded = false;
	}
}
