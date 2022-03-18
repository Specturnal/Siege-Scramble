using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Animator anim;

	private void Awake(){
		anim = GetComponent<Animator> ();
	}

	public void Shake(){
		anim.SetFloat ("Blend", Random.Range (0f, 1f));
		anim.SetTrigger ("Shake");
	}

	public void FollowPlayer(float x){
		if (x >= -22.5 && x <= 22.5)
			transform.position = new Vector3 (x, 2, -10);
	}
}
