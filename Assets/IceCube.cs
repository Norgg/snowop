using UnityEngine;
using System.Collections;

public class IceCube : MonoBehaviour {
	Vector3 startingPos;
	Quaternion startingRotation;
	int startTimer = 60;
	Spawner spawner;

	// Use this for initialization
	void Start () {
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		startingPos = transform.position;
		startingRotation = transform.rotation;
	}

	void OnTriggerStay(Collider collider) {
		if (rigidbody.isKinematic) return;
		OnTriggerEnter(collider);
	}

	void OnTriggerEnter(Collider collider) {
		spawner.fireThingSpawnChance += 0.002f;
		if (startTimer == 0 && collider.gameObject.name == "Base") {
			rigidbody.isKinematic = true;
			transform.position = startingPos;
			transform.rotation = startingRotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(Physics.gravity * rigidbody.mass * 5);
		if (startTimer > 0) startTimer--;
	}
}
