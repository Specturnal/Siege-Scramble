using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleSpawn : MonoBehaviour {

	[SerializeField]private GameObject Troll;
	private float SpawnRate, MaxSpawnrate;
	private Transform Spawnpoint;
	private Profile profile;
	private int CakeScore, SpawnLimit;

	public List<GameObject> Spawnlist = new List<GameObject> ();

	private void Start(){
		Spawnpoint = transform.GetChild (0);
		SpawnRate = 0;
		MaxSpawnrate = 1;
		SpawnLimit = 10;
		profile = FindObjectOfType<Profile> ();
	}

	private void Update(){
		if (SpawnRate <= 0 && Spawnlist.Count < SpawnLimit) {
			Spawnlist.Add (Instantiate (Troll, Spawnpoint.position, Quaternion.identity));
			SpawnRate = MaxSpawnrate;
		}
		SpawnRate -= Time.deltaTime;
		/*if (profile.AttackedTroll) {
			MaxSpawnrate = 0.75f;
		} else if (profile.AttackedCake) {
			MaxSpawnrate = 1.25f;
			SpawnLimit = 6;
		}*/

		for (int i = 0; i < Spawnlist.Count; i++) {
			if (Spawnlist [i] == null)
				Spawnlist.Remove (Spawnlist [i]);
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Entity")) {
			CakeScore++;
			Destroy (other.gameObject);
			if (CakeScore >= 30) {
				//profile.TriggerGameOver (1, "Haha, You got tricked! You are not supposed to betray your nation!");
			}
		}
	}
}
