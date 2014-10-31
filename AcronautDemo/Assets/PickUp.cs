using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	
	public SpriteRenderer sprite;
	public BoxCollider2D box;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {

			// Missing an animation for the balloon popping, but this will make the balloon disappear
			sprite.enabled = false;
			box.enabled = false;

		}
		
	}
}
