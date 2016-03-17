using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour {
	Text timer;
	public float timeRemaining = 300f; // 5 minutes in seconds
	public static float pickupTime = 5f;
	public static float dropOffTime = 10f;
	private GameObject canvas;

	void Start () {
		canvas = GameObject.Find ("Canvas");
		Text[] textValue = canvas.GetComponentsInChildren<Text> ();
		timer = textValue [0];
		timer.text = "0:00";

		// Decrease the time remaining by 1 second every 1 second
		InvokeRepeating ("decreaseTimeRemaining", 1.0f, 1.0f);
	}
	
	void Update () {
		if (timeRemaining == 0) {
			//fetch score
			PlayerPrefs.SetInt("score", canvas.GetComponent<Score>().FinalScore);
			PlayerPrefs.Save();
			// The player has ran out of time, show the gameover screen
			Application.LoadLevel ("Game Over");
		}

		var roundedTime = Mathf.CeilToInt (timeRemaining);
		timer.text = string.Format ("{0:00}:{1:00}", roundedTime / 60, roundedTime % 60);
	}

	void decreaseTimeRemaining() {
		timeRemaining--;
	}

	public void increaseTime(float seconds) {
		timeRemaining += seconds;
	}
}
