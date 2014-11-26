using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public static Music instance = null;
	public AudioClip[] newMusic;

	// Use this for initialization
	void Start () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		}
		else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded() {
	
	}
}
