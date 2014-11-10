using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject player;
	public float followSpeed;

	private Transform playerT;
	private Vector3 moveTarget;

	// Use this for initialization
	void Start () {
		playerT = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		moveTarget = new Vector3 (playerT.position.x, playerT.position.y, transform.position.z);
		//transform.position = new Vector3(playerT.position.x, playerT.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, moveTarget, followSpeed * Time.deltaTime);
	}

	void OnTriggerStay2D(Collider2D coll){
		moveTarget = transform.position;
	}
	void OnTriggerExit2D(Collider2D coll){

	}

}
