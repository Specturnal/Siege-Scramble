using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

	public void CloseTarget(GameObject Target){
		Target.SetActive (false);
	}

	public void ResumeTime(){
		Time.timeScale = 1;
	}
}
