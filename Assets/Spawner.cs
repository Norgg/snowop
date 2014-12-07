using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject fireThing;

	int maxEnemies = 20;
	public float fireThingSpawnChance = 0f;

	float left;
	float right;

	AudioSource calmsong;
	AudioSource excitesong;
	bool firstSpawn = true;

	void Start () {
		GameObject leftWall = GameObject.Find("LeftWall");
		GameObject rightWall = GameObject.Find("RightWall");
		left = -41 * Screen.width / Screen.height;
		right = 41 * Screen.width / Screen.height;
		leftWall.transform.position = new Vector3(left, 25, 0);
		rightWall.transform.position = new Vector3(right, 25, 0);
		calmsong = GetComponents<AudioSource>()[0];
		excitesong = GetComponents<AudioSource>()[1];
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(0);
		} else if (Input.GetKeyDown(KeyCode.M)) {
			calmsong.mute = !calmsong.mute;
			excitesong.mute = !excitesong.mute;
		}

		if (Random.value < fireThingSpawnChance && transform.childCount < maxEnemies) {
			GameObject newThing = (GameObject)Instantiate(fireThing);
			newThing.transform.parent = transform;
			if (firstSpawn) {
				firstSpawn = false;
				calmsong.Stop();
				excitesong.Play();
			}
			int dir = Mathf.FloorToInt(Random.value * 4);
			switch(dir) {
			case 0:
				newThing.transform.position = new Vector3(left + 3f, 1f, Random.value * 90f - 45f);
				break;
			case 1:
				newThing.transform.position = new Vector3(right - 3f, 1f, Random.value * 90f - 45f);
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
