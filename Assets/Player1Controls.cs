using UnityEngine;
using System.Collections;

public class Player1Controls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float speed = 0.05f;
		Vector3 vel = new Vector3(Input.GetAxisRaw("Horizontal1"), 0f, Input.GetAxisRaw("Vertical1"));

		if (vel.magnitude > 0) {
		    this.transform.position += vel.normalized * speed;

			Vector3 rot = Quaternion.LookRotation(vel).eulerAngles;
			iTween.RotateTo(gameObject, rot, 0.3f);
		}
	}
}
