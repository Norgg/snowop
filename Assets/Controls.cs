using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal;
	public string vertical;
	public string fire;
	public float speed;
	public float fireSpeed;
	public int fireTime;

	public bool dead = false;
	float gravMult = 10f;
	float snowballCost = 0.02f;
	float snowGain = 0.005f;
	int fireTimer = 0;

	int minDeadTime = 60;
	int deadTimer = 0;

	public GameObject snowball;
	Terrain terrain;
	Transform head;
	public Transform holding;

	// Use this for initialization
	void Start () {
		terrain = Terrain.activeTerrain;
		head = transform.Find("Head");
		head.rigidbody.detectCollisions = false;
		// Duplicate terrain data so it doesn't mess with the editor version.
		/*terrain.terrainData = (TerrainData) Object.Instantiate(terrain.terrainData);
		TerrainCollider tc = Terrain.activeTerrain.gameObject.GetComponent<TerrainCollider>();
		tc.terrainData = terrain.terrainData;*/

		float[,] heights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
		for (int i = 0; i < terrain.terrainData.heightmapWidth; i++) {
			for (int j = 0; j < terrain.terrainData.heightmapHeight; j++) {
				heights[i,j] = 0.5f;
			}
		}
		terrain.terrainData.SetHeights(0, 0, heights);
	}

	void Die() {
		dead = true;
		head.parent = null;
		head.rigidbody.isKinematic = false;
		head.rigidbody.detectCollisions = true;
		rigidbody.constraints = RigidbodyConstraints.None;
		deadTimer = minDeadTime;
		head.rigidbody.AddExplosionForce(5000f, transform.position, 50f);
	}

	void Recover() {
		dead = false;
		transform.rotation = Quaternion.identity;
		head.parent = transform;
		head.localPosition = new Vector3(0, 0.71f, 0);
		head.localRotation = Quaternion.identity;
		head.rigidbody.isKinematic = true;
		head.rigidbody.detectCollisions = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	void OnCollisionEnter(Collision collision) {
		if (dead) {
			if (collision.transform == head && deadTimer == 0) {
				Recover();
			}
		} else {

			if (holding == null && (collision.gameObject.name == "Head" || collision.gameObject.name == "IceCube") && !collision.rigidbody.isKinematic) {
				holding = collision.transform;
				holding.rigidbody.isKinematic = true;
			} else if (collision.gameObject.name.StartsWith("Snowball")) {
				Die();
			} else if (collision.gameObject.name.StartsWith("FireThing")) {
				Die();
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.AddForce(Physics.gravity * rigidbody.mass * gravMult);
		head.rigidbody.AddForce(Physics.gravity * head.rigidbody.mass * gravMult);
		if (fireTimer > 0) fireTimer--;
		if (deadTimer > 0) deadTimer--;

		if (dead) {
		    return;
		}

		if (holding != null) {
			if (holding.rigidbody.detectCollisions) {
				holding.transform.position = transform.position + (transform.forward + new Vector3(0, 0.3f, 0)) * (transform.localScale.z + holding.transform.localScale.z);
				holding.transform.rotation = transform.rotation;
			} else {
				holding = null;
			}
		}

		Vector3 vel = new Vector3(Input.GetAxis(horizontal), 0f, Input.GetAxis(vertical));

		if (vel.magnitude > 1f) {
			vel /= vel.magnitude;
		}

		vel /= Mathf.Pow(transform.localScale.y, 2);
		float currentSpeed = speed;// - transform.localScale.y;
		if (speed < 0)  speed = 0;

		vel *= currentSpeed;

		if (vel.magnitude > 0.01f) {
			transform.rotation = Quaternion.LookRotation(vel);
			transform.position += vel;
		} else {
			rigidbody.angularVelocity *= 0.2f;
		}

		// Fire
		if (Input.GetAxis(fire) < 0 && fireTimer == 0 && transform.localScale.y > 1.5f) {
			fireTimer = fireTime;

			if (holding != null) {
				holding.rigidbody.isKinematic = false;
				holding.rigidbody.velocity = fireSpeed * transform.forward + new Vector3(0, 1, 0) / 4;
				holding = null;
			} else {
				GameObject newSnowball = (GameObject)Object.Instantiate(snowball);
				newSnowball.transform.localScale = transform.localScale/4;
				newSnowball.transform.position = transform.position + transform.forward * 0.6f * transform.localScale.y + new Vector3(0, 0.3f, 0) * transform.localScale.y;
				newSnowball.rigidbody.velocity = fireSpeed * transform.forward + new Vector3(0, 1, 0);
				transform.localScale -= new Vector3(snowballCost, snowballCost, snowballCost);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		// Set position based on terrain height
		/*Vector3 pos = transform.position;
		pos.y = 0.5f + terrain.SampleHeight(transform.position);
		transform.position = pos;*/

		// Remove snow from terrain.
		int mapX = Mathf.FloorToInt(((transform.position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth) - 1;
		int mapZ = Mathf.FloorToInt(((transform.position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight) - 1;

		if (mapX >= 0 && mapX < terrain.terrainData.heightmapWidth-2 && mapZ >= 0 && mapZ < terrain.terrainData.heightmapHeight-2) {
			float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 3, 3);
			if (heights[1,1] >= 0.31f) {
				heights[0,0] -= 0.02f;
				heights[0,2] -= 0.02f;
				heights[2,0] -= 0.02f;
				heights[2,2] -= 0.02f;

				heights[1,0] -= 0.05f;
				heights[1,2] -= 0.05f;
				heights[2,1] -= 0.05f;
				heights[0,1] -= 0.05f;

				heights[1,1] = 0.1f;

				terrain.terrainData.SetHeights(mapX, mapZ, heights);
				transform.localScale += new Vector3(snowGain, snowGain, snowGain);
			}
		}
	}
}
