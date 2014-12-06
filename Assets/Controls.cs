using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal = "Horizontal1";
	public string vertical = "Vertical1";
	public string fire = "Fire1";
	public float speed = 0.1f;
	Terrain terrain;

	// Use this for initialization
	void Start () {
		terrain = Terrain.activeTerrain;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = new Vector3(Input.GetAxis(horizontal), 0f, Input.GetAxis(vertical));

		if (vel.magnitude > 1f) {
			vel /= vel.magnitude;
		}

		if (vel.magnitude > 0.01f) {
			vel *= speed;
			//Vector3 rot = Quaternion.LookRotation(vel).eulerAngles;
			transform.rotation = Quaternion.LookRotation(vel);
			//iTween.RotateTo(gameObject, rot, 0f);

			transform.position += vel;
		} else {
			rigidbody.angularVelocity *= 0.2f;
		}

		// Set position based on terrain height
		Vector3 pos = transform.position;
		pos.y = 0.5f + terrain.SampleHeight(transform.position);
		transform.position = pos;

		// Remove snow from terrain.
		float[,,] element = new float[1,1,2];
		element[0,0,0] = 0;
		element[0,0,1] = 1;

		int mapX = (int)((transform.position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
		int mapZ = (int)((transform.position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;

		//terrain.terrainData.SetAlphamaps(mapX, mapZ, element);
	}
}
