﻿using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal;
	public string vertical;
	public string fire;
	public float speed;
	public float fireSpeed;
	public int fireTime;

	float gravMult = 5f;
	float snowballCost = 0.02f;
	float snowGain = 0.01f;
	int fireTimer = 0;
	public GameObject snowball;
	Terrain terrain;

	// Use this for initialization
	void Start () {
		terrain = Terrain.activeTerrain;
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
	
	// Update is called once per frame
	void FixedUpdate () {
		if (fireTimer > 0) fireTimer--;

		rigidbody.AddForce(Physics.gravity * rigidbody.mass * gravMult);

		Vector3 vel = new Vector3(Input.GetAxis(horizontal), 0f, Input.GetAxis(vertical));

		if (vel.magnitude > 1f) {
			vel /= vel.magnitude;
		}

		vel /= Mathf.Pow(transform.localScale.y, 2);

		if (vel.magnitude > 0.01f) {
			vel *= speed;
			//Vector3 rot = Quaternion.LookRotation(vel).eulerAngles;
			transform.rotation = Quaternion.LookRotation(vel);
			//iTween.RotateTo(gameObject, rot, 0f);

			transform.position += vel;
		} else {
			rigidbody.angularVelocity *= 0.2f;
		}

		// Fire
		if (Input.GetAxis(fire) < 0 && fireTimer == 0 && transform.localScale.y > 1.5f) {
			fireTimer = fireTime;
			GameObject newSnowball = (GameObject)Object.Instantiate(snowball);
			newSnowball.transform.localScale = transform.localScale/4;
			newSnowball.transform.position = transform.position + transform.forward * 0.6f * transform.localScale.y + new Vector3(0, 0.3f, 0) * transform.localScale.y;
			newSnowball.rigidbody.velocity = fireSpeed * transform.forward + new Vector3(0, 1, 0);
			transform.localScale -= new Vector3(snowballCost, snowballCost, snowballCost);
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
