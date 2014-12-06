using UnityEngine;
using System.Collections;

public class Player1Controls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		float speed = 0.05f;
		if (Input.GetKey(KeyCode.D)) {
			pos.x += speed;
		} else if (Input.GetKey(KeyCode.A)) {
			pos.x -= speed;
		} else if (Input.GetKey(KeyCode.S)) {
			pos.z -= speed;
		} else if (Input.GetKey(KeyCode.W)) {
			pos.z += speed;
		}
		this.transform.position = pos;
	}
}
