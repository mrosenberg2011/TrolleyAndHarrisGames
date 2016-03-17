using System;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class AICharacterControl : MonoBehaviour
{
	public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
	public ThirdPersonCharacter character { get; private set; } // the character we are controlling
	public float trolleyChance = 0.10f;
	private GameObject[] targets;
	private bool assigned;
	private GameObject[] trolleyStops;
	private GameObject target;
	private GameObject destinationStop;
	private bool despawn;

	// Accessors
	public GameObject DestinationStop {
		get {
			return destinationStop;
		}
	}

	public GameObject Target {
		get {
			return target;
		}
		set {
			target = value;
			agent.SetDestination (target.transform.position);
		}
	}

	// Use this for initialization
	private void Start ()
	{
		targets = GameObject.FindGameObjectsWithTag ("Respawn");
		trolleyStops = GameObject.FindGameObjectsWithTag ("TrolleyStop");
		target = pickRandomTarget ();
		despawn = false;
		assigned = false;

		// get the components on the object we need ( should not be null due to require component so no need to check )
		agent = GetComponentInChildren<NavMeshAgent> ();
		character = GetComponent<ThirdPersonCharacter> ();

		agent.updateRotation = true;
		agent.updatePosition = true;

		agent.SetDestination (target.transform.position);
	}

	private GameObject pickRandomTarget ()
	{
		var chance = UnityEngine.Random.value;
		GameObject target;
		if (chance >= trolleyChance) {
			// Choose a waypoint as a destination
			target = targets [UnityEngine.Random.Range (0, targets.Length)];
			if (gameObject.transform.position == target.transform.position) {
				return pickRandomTarget ();
			}
		} else {
			// Choose a trolley stop as a destination
			target = trolleyStops [UnityEngine.Random.Range (0, trolleyStops.Length)];
			if (target.GetComponent<TrolleyStop> ().isStopOccupied ()) {
				// Occupied
				return pickRandomTarget ();
			} else if (gameObject.transform.position == target.transform.position) {
				// Already there
				return pickRandomTarget ();
			}
		}
		return target;
	}

	// Update is called once per frame
	private void Update ()
	{
		if (target != null) {	
			// use the values to move the character
			character.Move (agent.desiredVelocity, false, false);

			if (pathComplete ()) {
				if (target.tag.Equals ("TrolleyStop")) {
					if (!assigned) {
						// Wait for the trolley to pick person up
						target.GetComponent<TrolleyStop>().AssignedPeople.Add(this.gameObject);
						gameObject.GetComponent<ThirdPersonCharacter>().isDestroyable = false;
						// Pick a new trolley stop to be dropped off at
						do {

							destinationStop = trolleyStops [UnityEngine.Random.Range (0, trolleyStops.Length)];
						} while (destinationStop.Equals(target));

						assigned = true;
					}
				} else {
					// Reached walking destination, despawn
					despawn = true;
				}
			}
		} else {
			// We still need to call the character's move function, but we send zeroed input as the move param.
			character.Move (Vector3.zero, false, false);
		}

		if (despawn) {
			var campus = GameObject.Find ("Campus");
			campus.GetComponent<PeopleGenerator>().destroyPerson(gameObject);
		}
	}

	protected bool pathComplete ()
	{
		if (Vector3.Distance (agent.destination, agent.transform.position) <= agent.stoppingDistance) {
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
				return true;
			}
		}
			
		return false;
	}
}