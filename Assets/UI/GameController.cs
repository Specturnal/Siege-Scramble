using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController GMinstance;
	public int HeroClass;
	public int Ending;

	// Use this for initialization
	void Start () {
		//Screen.SetResolution (1280, 720, true);
		DontDestroyOnLoad (gameObject);
		if (GMinstance == null)
			GMinstance = this;
		else if (GMinstance != this)
			Destroy (gameObject);
	}

	public void SelectClass (int heroClass){
		HeroClass = heroClass;
		SceneManager.LoadScene ("MainScene");
	}

	public void ViewEnding(){
		SceneManager.LoadScene ("Ending");
	}

	public void RemoveInstance(){
		GMinstance = null;
		Destroy (gameObject);
	}


	public void QuitGame(){
		Application.Quit ();
	}
}
