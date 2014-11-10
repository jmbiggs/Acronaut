using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject player;
	public float followSpeed;
	public float leadAmount;

	private Transform playerT;
	private Vector3 moveTarget;
	private bool needToMove;
	private PlayerController pc;

	// Use this for initialization
	void Start () {
		playerT = player.transform;
		moveTarget = transform.position;
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(playerT.position.x, playerT.position.y, transform.position.z);

		if (needToMove) {
			Vector3 newTarget = new Vector3 (playerT.position.x, playerT.position.y, transform.position.z);

			float playerMoveAmt = pc.horizVelocity*Time.deltaTime + pc.horizTranslation;

			if (playerMoveAmt > 0)
			{
				newTarget.x += leadAmount;
			}
			else if (playerMoveAmt > 0)
			{
				newTarget.x -= leadAmount;
			}

			moveTarget = newTarget;

		}

		transform.position = Vector3.Lerp(transform.position, moveTarget, followSpeed * Time.deltaTime);
	}

	void OnTriggerStay2D(Collider2D coll){
		needToMove = false;
	}
	void OnTriggerExit2D(Collider2D coll){
		needToMove = true;
	}

}
