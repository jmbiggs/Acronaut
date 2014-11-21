using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	public string levelName;

	public float goldTime;
	public float silverTime;
	public float bronzeTime;

	public float playerTime;

	public WinPanel winPanel;

	public bool reachedGoal = false;

	// Use this for initialization
	void Start () {
		// winPanel = GameObject.FindGameObjectWithTag("WinPanel").GetComponent<WinPanel>();
		playerTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!reachedGoal)
			playerTime += Time.deltaTime;
	}

	public void Win() {
		winPanel.gameObject.SetActive(true);
		winPanel.DisplayWinPanel(goldTime, silverTime, bronzeTime, playerTime);
		reachedGoal = true;
	}
}
