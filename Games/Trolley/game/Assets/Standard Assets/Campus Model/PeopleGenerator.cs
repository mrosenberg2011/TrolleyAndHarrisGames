using UnityEngine;
using System.Collections.Generic;

public class PeopleGenerator : MonoBehaviour
{
	public GameObject[] spawnTypes;
	public int maxPeople;
	private static GameObject[] spawnPoints;
	private static List<GameObject> people;

	// Accessors
	public static GameObject[] Waypoints {
		get {
			return spawnPoints;
		}
	}

	public static List<GameObject> People {
		get {
			return people;
		}
	}

	// Use this for initialization
	void Start ()
	{
		people = new List<GameObject>();
		spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");

		// Generate people when the map loads with delay
		InvokeRepeating ("spawnPerson", 0.20f, 0.20f);
	}

	void spawnPerson ()
	{
		if (people.Count < maxPeople)
		{
			// Spawn a random person type at a random way point location
			GameObject spawnPoint = spawnPoints [UnityEngine.Random.Range (0, spawnPoints.Length)];
			var spawnType = spawnTypes[Random.Range(0, spawnTypes.Length)];
			var person = Instantiate (spawnType, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
			people.Add (person);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void destroyPerson (GameObject person)
	{
		person.SetActive (false);
		people.Remove (person);
		Destroy (person);
		Invoke ("spawnPerson", 0.20f);
	}
}