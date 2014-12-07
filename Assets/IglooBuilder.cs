using UnityEngine;
using System.Collections;

public class IglooBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Explode igloo.
		foreach (Transform child in transform) {
			if (child.gameObject.name.StartsWith("IceCube")) {
				child.rigidbody.isKinematic = false;
				float dir = Mathf.PI * 2 * Random.value;
				float dist = 30 + 40 * Random.value;
				Vector3 offset = new Vector3(Mathf.Sin(dir) * dist, 0, Mathf.Cos(dir) * dist);
				child.rigidbody.position += offset;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
