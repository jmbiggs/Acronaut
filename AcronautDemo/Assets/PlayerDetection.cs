using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour {

	public static int FOOT_DETECTOR = 0;
	public static int LEFT_DETECTOR = 1;
	public static int RIGHT_DETECTOR = 2;
	public static int HEAD_DETECTOR = 3;

	public GameObject player;
	public int type;


	void OnTriggerEnter2D (Collider2D other)
	{
		if (type == FOOT_DETECTOR) {
			if (player.GetComponent<PC>().grounded == false && player.GetComponent<PC>().jumpSpeed < 0f)
				player.GetComponent<PC>().grounded = true;
		}

	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (type == FOOT_DETECTOR) {
			player.GetComponent<PC>().grounded = false;
		}
	}
}
