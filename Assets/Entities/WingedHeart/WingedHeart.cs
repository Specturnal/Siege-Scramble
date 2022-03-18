using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedHeart : MonoBehaviour {

	private Enemy Core;
	private Animator anim;
	private Vector2 Direction;
	private float Countdown, y;

	private void Start(){
		Core = GetComponent<Enemy> ();
		anim = GetComponent<Animator> ();
		y = Random.Range (0.5f, 2.5f);
	}

	private void FixedUpdate(){
		
		Direction = new Vector2 (-1, 0);
		
		if (Core.rb.velocity.x > 0) {
			transform.localScale = new Vector2 (1, 1);
		} else if (Core.rb.velocity.x < 0) {
			transform.localScale = new Vector2 (-1, 1);
		}

		Core.rb.AddForce (Direction * Core.speed);
		if (Core.rb.velocity.x > Core.speed) {
			Core.rb.velocity = new Vector2 (Core.speed, Core.rb.velocity.y);
		}
		if (Core.rb.velocity.y > 10) {
			Core.rb.velocity = Vector2.zero;
		}

		transform.position = Vector2.Lerp (transform.position, new Vector2 (transform.position.x, y), 1);
	}
}
