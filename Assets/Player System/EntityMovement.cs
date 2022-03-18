using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour {

	private Enemy Core;
	private Animator anim;
	private Vector2 Direction;
	private float Countdown;

	private void Start(){
		Core = GetComponent<Enemy> ();
		anim = GetComponent<Animator> ();
	}

	private void FixedUpdate(){
		if (Core.Target != null) {
			if (Core.Target.position.x - transform.position.x > 0) {
				Direction = new Vector2 (1, 0);
				transform.localScale = new Vector2 (1, 1);
			} else if (Core.Target.position.x - transform.position.x < 0) {
				Direction = new Vector2 (-1, 0);
				transform.localScale = new Vector2 (-1, 1);
			}
		} else {
			/*if (Core.team == 1) {
				Direction = new Vector2 (-1, 0);
			} else {
//				if (Countdown <= 0) {
//					Countdown = 2;
//					Direction = new Vector2 (Random.Range (-1, 3), 0);
//				} else {
//					Countdown -= Time.deltaTime;
//				}
//				if (Mathf.Abs(Core.rb.velocity.x) >= 1) {
//					anim.SetBool ("Walk", true);
//				} else {
//					anim.SetBool ("Walk", false);
//				}
				Direction=new Vector2 (1, 0);
				anim.SetBool ("Walk", true);
			}
			if (Core.rb.velocity.x > 0) {
				transform.localScale = new Vector2 (1, 1);
			} else if (Core.rb.velocity.x < 0) {
				transform.localScale = new Vector2 (-1, 1);
			}
			*/
		}
			
		Core.rb.AddForce (Direction * Core.speed);
		if (Core.rb.velocity.x > Core.speed) {
			Core.rb.velocity = new Vector2 (Core.speed, Core.rb.velocity.y);
		}
		if (Core.rb.velocity.y > 10) {
			Core.rb.velocity = Vector2.zero;
		}
	}
}
