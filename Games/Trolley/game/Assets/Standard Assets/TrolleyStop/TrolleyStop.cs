using UnityEngine;
using System.Collections.Generic;

public class TrolleyStop : MonoBehaviour {
	private List<GameObject> assignedPeople;
	private bool assignedStop;
	private bool illuminate;
	private Light myLight;
	private GameObject arrow;

	// Accessors
	public List<GameObject> AssignedPeople {
		get { return assignedPeople; }
	}

	public bool AssignedStop {
		get { return assignedStop; }
		set { assignedStop = value; }
	}

	// Use this for initialization
	void Start () {

		myLight = GetComponentInChildren<Light> ();
		myLight.enabled = false;
		assignedPeople = new List<GameObject> ();
		assignedStop = false;
		arrow = transform.FindChild ("Arrow").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		illuminateStop ();
	}

	void illuminateStop() {
		if (assignedPeople.Count >= 1) { //someone is waiting

			myLight.enabled = true;
			arrow.SetActive (true);

			if (assignedStop) { //pickup and drop

				changeColor (Color.yellow);
			} else { //only pickup

				changeColor (Color.green);
			}
		} else if (assignedStop) { // drop off only
			myLight.enabled = true;
			arrow.SetActive(true);
			changeColor(Color.red);
		} else { // no one to pick up or drop off
			myLight.enabled = false;
			arrow.SetActive(false);
		}
	}

	public bool isStopOccupied() {
		return assignedPeople.Count >= 5;
	}

	void changeColor(Color newColor) {
		myLight.color = newColor;
	}
}
