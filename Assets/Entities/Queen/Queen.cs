using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour {

	private Enemy Core;
	private Animator anim;
	private Vector2 Direction;
	private float Countdown;
	[SerializeField]private GameObject Spark;
	private Transform FirePos;

	private void Start(){
		Core = GetComponent<Enemy> ();
		anim = GetComponent<Animator> ();
		FirePos = transform.GetChild (0);
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
			
			Direction = new Vector2 (-1, 0);
			if (Core.rb.velocity.x > 0) {
				transform.localScale = new Vector2 (1, 1);
			} else if (Core.rb.velocity.x < 0) {
				transform.localScale = new Vector2 (-1, 1);
			}

		}

		if (Countdown <= 0) {
			anim.Play ("Queen_Shoot");
			Instantiate (Spark, FirePos.position, Quaternion.identity).GetComponent<Projectile> ().SetupProjectileLaunch (Direction.x, 0);
			Countdown = Random.Range(1f,3f);
		}
		Countdown -= Time.deltaTime;


		Core.rb.AddForce (Direction * Core.speed);
		if (Core.rb.velocity.x > Core.speed) {
			Core.rb.velocity = new Vector2 (Core.speed, Core.rb.velocity.y);
		}
		if (Core.rb.velocity.y > 10) {
			Core.rb.velocity = Vector2.zero;
		}
	}
}
