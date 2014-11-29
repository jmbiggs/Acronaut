using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject player;
	public float followSpeed;
	public float leadAmount;

	private Transform playerT;
	private Vector3 moveTarget;
	private bool needToMove = true;
	private PlayerController pc;

	// Use this for initialization
	void Start () {
		playerT = player.transform;
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		//transform.position = new Vector3 (playerT.position.x, playerT.position.y, -10);
	}
	
	// Update is called once per frame
	void Update () {
		if (needToMove) {
			Vector3 newTarget = new Vector3 (playerT.position.x, playerT.position.y, -10);
			//float playerMoveAmt = pc.horizVelocity * Time.deltaTime + pc.horizTranslation;
			/*if (playerMoveAmt > 0)				
			{
				print(playerMoveAmt);
				newTarget.x += leadAmount;
			}
			else if (playerMoveAmt > 0)			
				newTarget.x -= leadAmount;*/

			if (pc.facingRight)
				newTarget.x += leadAmount;
			else
				newTarget.x -= leadAmount;

			moveTarget = newTarget;		
		}
		/*else
		{
			moveTarget = transform.position;
		}*/
		transform.position = Vector3.Lerp(transform.position, moveTarget, followSpeed * Time.deltaTime);	
	}

	void OnTriggerStay2D(Collider2D coll){
		needToMove = false;
	}
	void OnTriggerExit2D(Collider2D coll){
		needToMove = true;
	}

}
