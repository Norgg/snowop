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
		other.particleSystem.GetCollisionEvents(gameObject, events);
		foreach (ParticleSystem.CollisionEvent collision in events) {
			int mapX = Mathf.FloorToInt(((collision.intersection.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
			int mapZ = Mathf.FloorToInt(((collision.intersection.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);
			if (mapX >= 0 && mapX < terrain.terrainData.heightmapWidth && mapZ >= 0 && mapZ < terrain.terrainData.heightmapHeight) {
				float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 1, 1);
				if (heights[0,0] < 0.5f) {
					heights[0,0] = 0.5f;
					terrain.terrainData.SetHeights(mapX, mapZ, heights);
				} else if (heights[0,0] < 0.9f) {
					heights[0,0] += 0.1f;
					terrain.terrainData.SetHeights(mapX, mapZ, heights);
				}
			}
		}
	}
}
