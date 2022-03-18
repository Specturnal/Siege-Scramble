using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {

	private GameController GM; 

	[SerializeField]private GameObject Ending0, Ending1, Ending2;

	private void Start(){
		GM = FindObjectOfType<GameController> ();
		switch (GM.Ending) {
		case 0:
			Ending0.SetActive (true);
			break;
		case 1:
			Ending1.SetActive (true);
			break;
		default:
			Ending2.SetActive (true);
			break;
		}
		GM.RemoveInstance ();
	}

	public void ReturnToMenu(){
		SceneManager.LoadScene ("Menu");
		Time.timeScale = 1;
	}
}
