using UnityEngine;
using System.Collections;

public class SnowFallCollision : MonoBehaviour {
	Terrain terrain;
	ParticleSystem.CollisionEvent[] events;

	// Use this for initialization
	void Start () {
		terrain = Terrain.activeTerrain;
		events = new ParticleSystem.CollisionEvent[10];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other) {
		int collisions = other.particleSystem.GetCollisionEvents(gameObject, events);
		for (int i = 0; i < collisions; i++) {
			ParticleSystem.CollisionEvent collision = events[i];
			int mapX = 1+Mathf.FloorToInt(((collision.intersection.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
			int mapZ = 1+Mathf.FloorToInt(((collision.intersection.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);
			if (mapX >= 0 && mapX < terrain.terrainData.heightmapWidth-2 && mapZ >= 0 && mapZ < terrain.terrainData.heightmapHeight-2) {
				float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 3, 3);
				if (heights[1,1] < 0.5f) {
					if (heights[0,0] < 0.35) heights[0,0] = 0.35f + Random.value * 0.2f;
					if (heights[0,2] < 0.35) heights[0,0] = 0.35f + Random.value * 0.2f;
					if (heights[2,0] < 0.35) heights[0,0] = 0.35f + Random.value * 0.2f;
					if (heights[2,2] < 0.35) heights[0,0] = 0.35f + Random.value * 0.2f;
					
					if (heights[1,0] < 0.5) heights[1,0] = 0.4f + Random.value * 0.2f;
					if (heights[1,2] < 0.5) heights[1,0] = 0.4f + Random.value * 0.2f;
					if (heights[2,1] < 0.5) heights[1,0] = 0.4f + Random.value * 0.2f;
					if (heights[0,1] < 0.5) heights[1,0] = 0.4f + Random.value * 0.2f;
					
					heights[1,1] = 0.5f + Random.value * 0.1f;
						
					terrain.terrainData.SetHeights(mapX, mapZ, heights);
				} else if (heights[1,1] < 0.9f) {
					heights[1,1] += 0.1f;
					terrain.terrainData.SetHeights(mapX, mapZ, heights);
				}
			}
		}
	}
}
