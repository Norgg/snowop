using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject fireThing;
	public float fireThingSpawnChance;
	GameObject p1;
	GameObject p2;
	Controls p1c;
	Controls p2c;

	// Use this for initialization
	void Start () {
		p1 = GameObject.Find("Snowman1");
		p2 = GameObject.Find("Snowman2");
		p1c = p1.GetComponent<Controls>();
		p2c = p2.GetComponent<Controls>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(0);
		}

		if (Random.value < fireThingSpawnChance) {
			GameObject newThing = (GameObject)Instantiate(fireThing);
			int dir = Mathf.FloorToInt(Random.value * 4);
			switch(dir) {
			case 0:
				newThing.transform.position = new Vector3(-83f, 1f, Random.value * 90f - 45f);
				break;
			case 1:
				newThing.transform.position = new Vector3(83f, 1f, Random.value * 90f - 45f);
				break;
			case 2:
				newThing.transform.position = new Vector3(Random.value * 160f - 80f, 1f, 42f);
				break;
			case 3:
				newThing.transform.position = new Vector3(Random.value * 160f - 80f, 1f, -42f);
				break;
			}
		}
	}
}
