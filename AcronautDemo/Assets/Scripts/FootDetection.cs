using UnityEngine;
using System.Collections;

public class FootDetection : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		print ("in");
		GameObject player = GameObject.FindWithTag ("Player");
		player.GetComponent<Player> ().grounded = true;
		print (player.GetComponent<Player> ().grounded);
	}
	void OnTriggerExit2D (Collider2D other)
	{
		print ("out");
		GameObject player = GameObject.FindWithTag ("Player");
		player.GetComponent<Player> ().grounded = false;
		print (player.GetComponent<Player> ().grounded);
	}

}
