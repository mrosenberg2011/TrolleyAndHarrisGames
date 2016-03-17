using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScores : MonoBehaviour {

	private int fetchedScore;
	private Text scoreText;

	// Use this for initialization
	void Start () {
		fetchedScore = PlayerPrefs.GetInt ("score");

		GameObject canvas = GameObject.Find ("Canvas");
		Text[] textValue = canvas.GetComponentsInChildren<Text> ();
		scoreText = textValue [1];
		
		if (scoreText != null) {
			scoreText.text = "Final Score: " + fetchedScore;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
