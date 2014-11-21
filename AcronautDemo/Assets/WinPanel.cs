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
		string minutes;
		if ((int) time / 60 == 0)
			minutes = "00";
		else 
			minutes = ((int) time / 60).ToString();
		string seconds = Mathf.FloorToInt(time % 60).ToString();
		string milliseconds = Mathf.FloorToInt((time % 1f) * 100f).ToString();
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
