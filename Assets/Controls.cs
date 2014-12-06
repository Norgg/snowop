using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal = "Horizontal1";
	public string vertical = "Vertical1";
	public string fire = "Fire1";

	public GameObject terrain;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float speed = 0.05f;
		Vector3 vel = new Vector3(Input.GetAxisRaw(horizontal), 0f, Input.GetAxisRaw(vertical));

		if (vel.magnitude > 0) {
		    this.transform.position += vel.normalized * speed;

			Vector3 rot = Quaternion.LookRotation(vel).eulerAngles;
			iTween.RotateTo(gameObject, rot, 0.3f);
		}
		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + 0.5f;
		transform.position = pos;
	}
}
