using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

	public string levelName;
	public string nextLevel;

	public float goldTime;
	public float silverTime;
	public float bronzeTime;

	public float playerTime;

	public WinPanel winPanel;

	public Text timeText;

	public bool reachedGoal = false;

	// Use this for initialization
	void Start () {
		// winPanel = GameObject.FindGameObjectWithTag("WinPanel").GetComponent<WinPanel>();
		playerTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (reachedGoal) {
			if (Input.GetButtonDown ("Jump")) {
				Application.LoadLevel(nextLevel);
			}
		}


		if (playerTime < 0f)
			playerTime = 0f;
		if (!reachedGoal) {
			playerTime += Time.deltaTime;

			int minuteInt = (int) playerTime / 60;
			string minutes;
			if (minuteInt < 10)
				minutes = "0" + minuteInt.ToString();
			else 
				minutes = minuteInt.ToString();
			
			int secondInt = Mathf.FloorToInt(playerTime % 60);
			string seconds;
			if (secondInt < 10)
				seconds = "0" + secondInt.ToString (); 
			else 
				seconds = secondInt.ToString (); 
			
			int millisecondInt = Mathf.FloorToInt((playerTime % 1f) * 100f);
			string milliseconds;
			if (millisecondInt < 10)
				milliseconds = "0" + millisecondInt.ToString();
			else
				milliseconds = millisecondInt.ToString();
			
			string formattedTime = (minutes + "'" + seconds + "'" + milliseconds);
			timeText.text = formattedTime;
		}
	}

	public void Win() {
		winPanel.gameObject.SetActive(true);
		winPanel.DisplayWinPanel(goldTime, silverTime, bronzeTime, playerTime);
		reachedGoal = true;
	}


}
