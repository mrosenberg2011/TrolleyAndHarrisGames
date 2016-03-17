using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
	private int score;
	private Text scoreText;
	public static int pickup = 10;
	public static int dropoff = 50;
	public static int kill = 1;

	public int FinalScore {
		get { return score; }
	}

	// Use this for initialization
	void Start () {
		score = 0;
		GameObject canvas = GameObject.Find ("Canvas");
		Text[] textValue = canvas.GetComponentsInChildren<Text> ();
		scoreText = textValue [1];

		if (scoreText != null) {
			scoreText.text = "Score: " + score;
		}

		if (scoreText == null) {
			Debug.Log ("Cannot find 'ScoreText' script");
		}
	}
	
	// Update is called once per frame
	public void UpdateScore (int s) {
		score = score + s;
		scoreText.text = "Score: " + score;
	}
}
