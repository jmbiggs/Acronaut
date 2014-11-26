using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudienceGauge : MonoBehaviour {

	public Text text;

	public float value;
	public float decayRate;
	public bool isPaused;

	float lerpValue;

	ArrayList lerpValues;


	// Use this for initialization
	void Start () {
		lerpValues = new ArrayList();
		value = 50f;
		isPaused = false;
		lerpValue = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused && value > 0f) {
			value -= decayRate * Time.deltaTime;

			foreach (Bonus bonus in lerpValues) {
				value += bonus.value * Time.deltaTime;
				bonus.timeRemaining -= Time.deltaTime;
				if (bonus.timeRemaining < 0f) {
					lerpValues.Remove(bonus);
				}
			}
			/*
			if (lerpValue > 0f) {
				value += lerpValue * Time.deltaTime;
				lerpValue -= Time.deltaTime;
				if (lerpValue < 0f)
					lerpValue = 0f;
			}
			*/
			if (value < 0f)
				value = 0f;
		}
		text.text = "" + value;
	}

	// Instantly adds a vlaue to the gauge
	public void Add(float v) {
		value += v;
		if (value > 100f) {
			value = 100f;
		}
	}

	// Adds a value to the gauge over some period of time
	public void Lerp(float v, float t) {
		lerpValues.Add(new Bonus(v, t));
	}

	public class Bonus {
		public float value;
		public float timeRemaining;

		public Bonus(float v, float t) {
			value = v;
			timeRemaining = t;
		}
	}
}
