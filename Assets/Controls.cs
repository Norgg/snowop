using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal = "Horizontal1";
	public string vertical = "Vertical1";
	public string fire = "Fire1";
	public float speed = 0.05f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = new Vector3(Input.GetAxisRaw(horizontal), 0f, Input.GetAxisRaw(vertical));
		vel *= speed;

		if (vel.magnitude > 0) {
		    this.transform.position += vel.normalized * speed;

			Vector3 rot = Quaternion.LookRotation(vel).eulerAngles;
			iTween.RotateTo(gameObject, rot, 0.3f);
		}
	}
}
