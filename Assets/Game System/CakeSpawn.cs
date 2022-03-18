using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeSpawn : MonoBehaviour {
	
	[SerializeField]private GameObject[] Enemies;
	private float SpawnRate, MaxSpawnRate;
	private Transform Spawnpoint;
	private Profile Profile;
	[SerializeField]private GameObject TheQueen;
	private bool SpawnedQueen;
	private GameObject QueenObject;

	private void Start(){
		Spawnpoint = transform.GetChild (0);
		SpawnRate = 0;
		MaxSpawnRate = 1;
		Profile = FindObjectOfType<Profile> ();
	}

	private void Update(){
		if (SpawnRate <= 0) {
			Instantiate (Enemies [Random.Range (0, Enemies.Length)], Spawnpoint.position, Quaternion.identity);
			SpawnRate = MaxSpawnRate;
		}
		/*if (Profile.AttackedCake) {
			MaxSpawnRate = 0.75f;
		} else if (Profile.AttackedTroll) {
			MaxSpawnRate = 1.25f;
		}
		SpawnRate -= Time.deltaTime;

		if (Profile.CakeKilled >= 50 && !SpawnedQueen) {
			QueenObject = Instantiate (TheQueen, Spawnpoint.position, Quaternion.identity);
			SpawnedQueen = true;
		}
		if (SpawnedQueen) {
			if (QueenObject == null) {
				Profile.TriggerGameOver (2, "You defeated the queen and saved the kingdom!");
			}
		}*/
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Entity")) {
			Destroy (other.gameObject);
		}
	}
}
