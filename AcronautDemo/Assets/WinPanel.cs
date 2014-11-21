using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour {

	public Image bronzeStar;
	public Image silverStar;
	public Image goldStar;
	public Text playerTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void DisplayWinPanel(float goldTime, float silverTime, float bronzeTime, float time) {
		int minuteInt = (int) time / 60;
		string minutes;
		if (minuteInt < 10)
			minutes = "0" + minuteInt.ToString();
		else 
			minutes = minuteInt.ToString();

		int secondInt = Mathf.FloorToInt(time % 60);
		string seconds;
		if (secondInt < 10)
			seconds = "0" + secondInt.ToString (); 
		else 
			seconds = secondInt.ToString (); 

		int millisecondInt = Mathf.FloorToInt((time % 1f) * 100f);
		string milliseconds;
		if (millisecondInt < 10)
			milliseconds = "0" + millisecondInt.ToString();
		else
			milliseconds = millisecondInt.ToString();

		string formattedTime = (minutes + "'" + seconds + "'" + milliseconds);
		playerTime.text = formattedTime;
		if (time <= goldTime) 
			goldStar.gameObject.SetActive(true);
		else if (time <= silverTime)
			silverStar.gameObject.SetActive(true);
		else if (time <= bronzeTime)
			bronzeStar.gameObject.SetActive(true);
	}
}
