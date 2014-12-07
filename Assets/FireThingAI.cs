using UnityEngine;
using System.Collections;

public class FireThingAI : MonoBehaviour {
	Terrain terrain;
	GameObject p1;
	GameObject p2;
	Controls p1c;
	Controls p2c;
	float speed = 0.1f;
	float maxSpeed = 8f;
	int hp = 3;
	float pulsecounter = 0f;

	// Use this for initialization
	void Start () {
		terrain = Terrain.activeTerrain;
		p1 = GameObject.Find("Snowman1");
		p2 = GameObject.Find("Snowman2");
		p1c = p1.GetComponent<Controls>();
		p2c = p2.GetComponent<Controls>();
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name.StartsWith("Snowball")) {
			hp--;
			if (hp == 0) {
				Destroy(gameObject);
			} else {
				audio.Play();
				transform.localScale *= 0.8f;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		renderer.material.mainTextureOffset += new Vector2(Random.value/100f, Random.value/100f);
		rigidbody.angularVelocity = new Vector3(0, 25, 0);

		Vector3 pos = transform.position;
		pos.y = 0.3f + transform.localScale.y + terrain.SampleHeight(transform.position);
		transform.position = pos;

		// Remove snow from terrain.
		int mapX = Mathf.FloorToInt(((transform.position.x - terrain.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth) - 1;
		int mapZ = Mathf.FloorToInt(((transform.position.z - terrain.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight) - 1;

		pulsecounter+=0.1f;
		float pulsescale = hp * Mathf.Sin(pulsecounter) / 30f;
		transform.localScale += new Vector3(pulsescale, 0, pulsescale);

		if (mapX >= 0 && mapX < terrain.terrainData.heightmapWidth-2 && mapZ >= 0 && mapZ < terrain.terrainData.heightmapHeight-2) {
			float[,] heights = terrain.terrainData.GetHeights(mapX, mapZ, 3, 3);
			if (heights[1,1] >= 0.31f) {
				heights[0,0] -= 0.05f;
				heights[0,2] -= 0.05f;
				heights[2,0] -= 0.05f;
				heights[2,2] -= 0.05f;
				
				heights[1,0] -= 0.1f;
				heights[1,2] -= 0.1f;
				heights[2,1] -= 0.1f;
				heights[0,1] -= 0.1f;
				
				heights[1,1] = 0.1f;
				
				terrain.terrainData.SetHeights(mapX, mapZ, heights);
			}
		}

		if (p2c.dead || Vector3.Distance(transform.position, p1.transform.position) < Vector3.Distance(transform.position, p2.transform.position)){
			if (!p1c.dead) MoveTowards(p1);
		} else {
			MoveTowards(p2);
		}
	}

	void MoveTowards(GameObject player) {
		rigidbody.velocity += (player.transform.position - transform.position).normalized * speed;
		if (rigidbody.velocity.magnitude > maxSpeed) {
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		}
	}
}
