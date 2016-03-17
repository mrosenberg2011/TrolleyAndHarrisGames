using UnityEngine;
using System.Collections.Generic;

public class Trolley : MonoBehaviour
{
	public float trolleyStopDistance = 10.0f;
	public float trolleyStopTime = 4.0f;
	private List<GameObject> passengers;
	private GameObject trolley;
	private Score score;
	private CountdownTimer time;

	private GameObject currentTrolleyStop;
	private float currentTrolleyStopTime;

	// Use this for initialization
	void Start ()
	{
		passengers = new List<GameObject> ();
		trolley = GameObject.FindGameObjectWithTag ("Player");
		score = FindObjectOfType<Score> ();
		time = (CountdownTimer)FindObjectsOfType (typeof(CountdownTimer)) [0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (atTrolleyStop()) {
			List<GameObject> passengersToRemove = new List<GameObject>();

			// Drop off all passengers with this trolley stop as their destination
			foreach (GameObject passenger in passengers) {
				var dest = passenger.GetComponent<AICharacterControl> ().DestinationStop;
				if (dest.Equals (currentTrolleyStop)) {
					// Move the passenger from their old location to the Trolley's current location, adjusted in the x axis
					passenger.transform.position = new Vector3 (trolley.transform.position.x + 5, trolley.transform.position.y, trolley.transform.position.z);
					// Enable the rendering of the passenger
					passenger.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
					// Set the navigation agent's destination to the nearest waypoint from here
					passenger.GetComponent<AICharacterControl> ().Target = closestWaypoint ();
					passengersToRemove.Add (passenger);
					dest.GetComponent<TrolleyStop>().AssignedStop = false;
					score.UpdateScore(Score.dropoff);
					time.increaseTime(CountdownTimer.dropOffTime);
				}
			}

			// Remove the assigned people from the trolley stop
			foreach (GameObject passenger in passengersToRemove) {
				passengers.Remove (passenger);
			}

			List<GameObject> toBeRemoved = new List<GameObject>();

			// Load all passengers with their destination set to this trolley stop
			foreach (GameObject person in currentTrolleyStop.GetComponent<TrolleyStop>().AssignedPeople) {
				// Disable the rendering of the person
				person.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
				person.GetComponent<ThirdPersonCharacter>().isDestroyable = false;
				passengers.Add (person);
				var dest = person.GetComponent<AICharacterControl> ().DestinationStop;
				dest.GetComponent<TrolleyStop>().AssignedStop = true;
				toBeRemoved.Add (person);
				score.UpdateScore(Score.pickup);
				time.increaseTime(CountdownTimer.pickupTime);
			}

			// Remove the assigned people from the trolley stop
			foreach (GameObject person in toBeRemoved) {
				currentTrolleyStop.GetComponent<TrolleyStop>().AssignedPeople.Remove (person);
			}
		}
	}

	// Determine if the trolley has waited nearby long enough
	internal bool atTrolleyStop() {
		if (nearestTrolleyStop () != null) {
			if (nearestTrolleyStop() == currentTrolleyStop) {
				// Update the time that the trolley has been near the current
				currentTrolleyStopTime -= Time.deltaTime;
				if ((currentTrolleyStopTime <= 0)/* && (currentTrolleyStop.GetComponent<TrolleyStop>().AssignedPeople.Count > 0)*/) {
					return true;
				}
			} else {
				// At a different trolley stop, restart the counter
				currentTrolleyStopTime = trolleyStopTime;
				currentTrolleyStop = nearestTrolleyStop();
				return false;
			}
		}

		return false;
	}

	// Determine the closest trolley stop
	internal GameObject nearestTrolleyStop() {
		GameObject result = null;
		var trolleyStops = GameObject.FindGameObjectsWithTag ("TrolleyStop");
		foreach (GameObject trolleyStop in trolleyStops) {
			var distance = Vector3.Distance (trolleyStop.transform.position, trolley.transform.position);
			if (distance <= trolleyStopDistance) {
				result = trolleyStop;
			}
		}
		return result;
	}

	// Iterate through all of the waypoints and determine the one with the smallest distance from here
	internal GameObject closestWaypoint ()
	{
		GameObject closest = null;
		float closestDistance = float.MaxValue;
		foreach (GameObject waypoint in PeopleGenerator.Waypoints) {
			var distance = Vector3.Distance (trolley.transform.position, waypoint.transform.position);
			if (distance < closestDistance) {
				// Found a closer waypoint, set it
				closest = waypoint;
				closestDistance = distance;
			}
		}

		return closest;
	}

	// Return the number of passengers in the  passagers List
	internal int getNumPassengers() {
		return passengers.Count;
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag ("TrolleyStop") || other.collider.CompareTag ("Immovable")) {

			gameObject.GetComponent<AudioSource>().Play();
		}
	}
}
