using UnityEngine;
using System.Collections;

public class IglooBuilder : MonoBehaviour {
	public bool iglooComplete = false;
	int startChildren;
	public bool p1Home = false;
	public bool p2Home = false;
	bool won = false;
	int wonTimer = 30;
	GameObject jet;

	void Start() {
		// Explode igloo.
		startChildren = transform.childCount;
		jet = GameObject.Find("Jet");

		foreach (Transform child in transform) {
			if (child.gameObject.name.StartsWith("IceCube")) {
				child.rigidbody.isKinematic = false;
				float dir = Mathf.PI * 2 * Random.value;
				float dist = 30 + 10 * Random.value;
				Vector3 offset = new Vector3(1.5f * Mathf.Sin(dir) * dist, 0, Mathf.Cos(dir) * dist);
				child.rigidbody.position += offset;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.childCount == startChildren) {
			iglooComplete = true;
		} else {
			iglooComplete = false;
		}

		if (p1Home && p2Home && iglooComplete && !won) {
			won = true;
			jet.particleSystem.Play();
			Camera.allCameras[0].transform.position -= Camera.allCameras[0].transform.forward * 50;
		}

		if (won) {
			if (wonTimer > 0) wonTimer--;
			else transform.position += new Vector3(0, 0.3f, 0);
		}
		p1Home = false;
		p2Home = false;
	}
}
