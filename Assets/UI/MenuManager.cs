using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public void OpenPanel(GameObject Panel){
		Panel.SetActive (true);
	}

	public void ClosePanel(GameObject Panel){
		Panel.SetActive (false);
	}
}
