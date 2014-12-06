using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour {
	Terrain terrain;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (terrain == null) {
			terrain = Terrain.activeTerrain;
		}
		int mapX = Mathf.FloorToInt(((transform.position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
		int mapZ = Mathf.FloorToInt(((transform.position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);
		if (mapX >= 0 && mapX < terrain.terrainData.heightmapWidth && mapZ >= 0 && mapZ < terrain.terrainData.heightmapHeight) {
			float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 1, 1);
			if (heights[0,0] < 0.9f) {
				heights[0,0] += 1f;
				terrain.terrainData.SetHeights(mapX, mapZ, heights);
			}
		}

		Destroy(gameObject);
	}
}
