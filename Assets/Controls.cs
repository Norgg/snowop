﻿using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public string horizontal;
	public string vertical;
	public string fire;
	public float speed = 0.1f;
	public int fireTime = 30;
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
	void Update () {
		if (fireTimer > 0) fireTimer--;

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

		// Fire
		if (Input.GetAxis(fire) < 0 && fireTimer == 0 && transform.localScale.y > 0.1) {
			fireTimer = fireTime;
			GameObject newSnowball = (GameObject)Object.Instantiate(snowball);
			newSnowball.transform.localScale = transform.localScale/4;
			newSnowball.transform.position = transform.position + transform.forward * transform.localScale.y + new Vector3(0, 0.3f, 0);
			newSnowball.rigidbody.velocity = 20 * transform.forward + new Vector3(0, 1, 0);
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
		int mapX = Mathf.FloorToInt(((transform.position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
		int mapZ = Mathf.FloorToInt(((transform.position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);

		float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 1, 1);
		if (heights[0,0] >= 0.5f) {
			heights[0,0] = 0.3f;
			terrain.terrainData.SetHeights(mapX, mapZ, heights);
			transform.localScale += new Vector3(snowGain, snowGain, snowGain);
		}
	}
}
