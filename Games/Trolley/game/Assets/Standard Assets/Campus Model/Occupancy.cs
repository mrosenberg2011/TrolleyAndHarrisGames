using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Occupancy : MonoBehaviour {
	private Text occupancy;
	public Trolley trolley;
	private int numPass;

	// Use this for initialization
	void Start () {
		numPass = 0;
		GameObject canvas = GameObject.Find ("Canvas");
		Text[] textValue = canvas.GetComponentsInChildren<Text> ();
		occupancy = textValue [2];
	}
	
	// Update is called once per frame
	void Update () {
		numPass = trolley.getNumPassengers ();
		//Debug.Log (numPass);
		occupancy.text = "Passengers: " + numPass;
	}
}
