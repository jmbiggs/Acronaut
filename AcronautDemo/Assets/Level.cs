using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	public string levelName;

	public float goldTime;
	public float silverTime;
	public float bronzeTime;

	public float playerTime;

	public GameObject winPanel;

	public bool reachedGoal = false;

	// Use this for initialization
	void Start () {
		playerTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!reachedGoal)
			playerTime += Time.deltaTime;
	}

	public void Win() {
		winPanel.SetActive(true);
		reachedGoal = true;
	}
}
