using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	public float speed = 10f;
	private Vector3 target;
	private static float LOW = 11.5f;
	private static float HIGH = 18f;

	// Use this for initialization
	void Start () {

		target = new Vector3 (transform.parent.position.x, LOW, transform.parent.position.z);
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y >= HIGH) {

			target = new Vector3 (transform.parent.position.x, LOW, transform.parent.position.z);
		}
		if (transform.position.y <= LOW) {

			target = new Vector3 (transform.parent.position.x, HIGH, transform.parent.position.z);
		}

		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target, step);

	}
}
