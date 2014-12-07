using UnityEngine;
using System.Collections;

public class IceCube : MonoBehaviour {
	Vector3 startingPos;
	Quaternion startingRotation;
	int startTimer = 60;
	Transform startParent;
	Spawner spawner;
	float spawnChancePerBlock = 0.001f;
	GameObject p1;
	GameObject p2;
	Controls p1c;
	Controls p2c;

	void Awake() {
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		startingPos = transform.position;
		startingRotation = transform.rotation;
		startParent = transform.parent;
	}

	void Start () {
		p1 = GameObject.Find("Snowman1");
		p2 = GameObject.Find("Snowman2");
		p1c = p1.GetComponent<Controls>();
		p2c = p2.GetComponent<Controls>();
	}

	void OnTriggerStay(Collider collider) {
		if (rigidbody.isKinematic) return;
		OnTriggerEnter(collider);
	}

	void OnTriggerEnter(Collider collider) {
		if (transform.parent == startParent) return;
		if (startTimer == 0 && collider.gameObject.name == "Base") {
			if (p1c.holding == transform) p1c.holding = null;
			if (p2c.holding == transform) p2c.holding = null;
			rigidbody.isKinematic = true;
			transform.position = startingPos;
			transform.rotation = startingRotation;
			transform.parent = startParent;
			spawner.fireThingSpawnChance += spawnChancePerBlock;
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (!rigidbody.isKinematic) transform.parent = null;
		rigidbody.AddForce(Physics.gravity * rigidbody.mass * 5);
		if (startTimer > 0) startTimer--;
	}
}
