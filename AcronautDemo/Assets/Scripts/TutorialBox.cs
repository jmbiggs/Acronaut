using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialBox : MonoBehaviour {

	public string textToDisplay;
	Text textBox;

	// Use this for initialization
	void Start () {
		textBox = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		textBox.text = textToDisplay;
		textBox.enabled = true;
	}

	void OnTriggerExit2D(Collider2D coll) {
		textBox.enabled = false;
	}
}
