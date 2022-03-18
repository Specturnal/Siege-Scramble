using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hitbox {

	[SerializeField] private float speed, lifeTime;
	[SerializeField] [Range(0, 100)] private int accuracy = 100;
	[SerializeField] [Tooltip("Destroys gameobject upon contact")] private bool destroyOnContact = true;
	[SerializeField] [Tooltip("Only applies for triggers")] private bool pierce = false, phase = false;
	[SerializeField] private TraversalMethods traversalMethod;
	[SerializeField] private Carryable.DirectionalOptions directionalOption = Carryable.DirectionalOptions.FaceLaunchDirection;
	public List<GameObject> objectsToLoadOnHit = new List<GameObject>();

	private Rigidbody2D rb;
	private Collider2D hitbox;
	private Vector2 direction, localScale;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		hitbox = GetComponent<Collider2D>();
	}

	private void Start() {
		if (lifeTime != 0) Destroy(gameObject, lifeTime);
	}

	private void FixedUpdate() {
		if(traversalMethod == TraversalMethods.UseVelocity) rb.velocity = direction * speed;
	}

	public void SetupProjectileProperties(float speed, float lifeTime = 0, bool destroyOnContact = true, bool pierce = false, bool phase = false,
		TraversalMethods traversalMethod = TraversalMethods.UseVelocity, Carryable.DirectionalOptions directionalOption = Carryable.DirectionalOptions.FaceLaunchDirection) =>
		(this.speed, this.lifeTime, this.destroyOnContact, this.pierce, this.phase, this.traversalMethod, this.directionalOption) =
		(speed, lifeTime, destroyOnContact, pierce, phase, traversalMethod, directionalOption);

	public void SetupProjectileLaunch(float x_launch, float y_launch, int accuracy = 100, float launchDelay = 0, float activeDelay = 0) {
		direction.Set(x_launch, y_launch);
		if (directionalOption != Carryable.DirectionalOptions.KeepDefaultDirection) {
			localScale.Set(x_launch * (int)directionalOption, 1);
			transform.localScale = localScale;
		}

		if (accuracy != 100) this.accuracy = accuracy;

		StartCoroutine(LaunchEnumerator(launchDelay, activeDelay));
	}

	private IEnumerator LaunchEnumerator(float launchDelay, float activeDelay) {
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		Collider2D collider2D = GetComponent<Collider2D>();
		collider2D.enabled = false;

		if (launchDelay > 0) yield return new WaitForSeconds(launchDelay);
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (traversalMethod == TraversalMethods.UseForce) rb.AddForce(direction * speed * 50);
		if (activeDelay > 0) yield return new WaitForSeconds(activeDelay);
		collider2D.enabled = true;
	}

	private void OnHit() {
		Destroy(gameObject);
		if (objectsToLoadOnHit.Count > 0)
			for (int i = 0; i < objectsToLoadOnHit.Count; i++)
				Instantiate(objectsToLoadOnHit[i], transform.position, Quaternion.identity);
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (!hitbox.isTrigger) return;

		if (accuracy == 100 || accuracy >= Random.Range(0, 101)) {
			base.OnTriggerEnter2D(other);
		} else {
			Debug.Log("Missed");
		}

		if (other.CompareTag("Entity")) {//collides with entity
			if (!pierce && (damageable as Entity).faction != faction)
				if (destroyOnContact) OnHit();
				else Destroy(this);
		} else if (!other.isTrigger) {//collides with any solid surface
			if (!phase) {
				if (destroyOnContact) OnHit();
				else Destroy(this);
			}
		}
	}
	private void OnCollisionEnter2D(Collision2D other) {
		if (hitbox.isTrigger) return;

		if (accuracy == 100 || accuracy >= Random.Range(0, 101)) {
			base.OnTriggerEnter2D(other.collider);
		} else {
			Debug.Log("Missed");
		}

		if (other.gameObject.CompareTag("Entity")) {
		} else if (other.gameObject.CompareTag("Ground")) {
		}

		if (destroyOnContact) OnHit();
		else Destroy(this);
	}

	public enum TraversalMethods {
		UseVelocity,
		UseForce
	}
}