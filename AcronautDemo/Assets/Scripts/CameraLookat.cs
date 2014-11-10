using UnityEngine;
using System.Collections;

public class CameraLookat : MonoBehaviour {

	public Vector3 target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target;
	}
}
