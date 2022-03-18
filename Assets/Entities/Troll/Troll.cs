using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : MonoBehaviour {

	private Enemy Core;
	private Animator anim;
	private bool Cooldown;

	private void Start(){
		anim = GetComponent<Animator> ();
		Core = GetComponent<Enemy> ();
	}

	private void Update (){
		if (Core.Target != null && !Cooldown) {
			StartCoroutine (Attack ());
		}
	}

	private IEnumerator Attack(){
		anim.Play ("TrollAttack");
		Cooldown = true;
		yield return new WaitForSeconds (1);
		Cooldown = false;
	}
}
