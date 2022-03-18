using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//change class name soon
public class Enemy : Entity {
	[SerializeField] private bool DamageOnContact;
	public float MaxHp;
	[SerializeField] private GameObject Coin, Heart;

	[HideInInspector] public Transform Target;

	private CameraController Cam;
	Profile profile;

	[SerializeField] private Entity player;
	private Vector2 UpwardsKnockback = new Vector2(0, 2);
	[SerializeField] private GameObject Blood;

	private AudioSource audios;

	public override event System.Action OnAttackInputDown, OnAttackInputHold, OnAttackInputUp, OnBlockInputDown, OnBlockInputUp;

	RaycastHit2D raycast;

	protected override void Awake() {
		base.Awake();

		health = MaxHp;
		//Cam = FindObjectOfType<CameraController>();
		//player = FindObjectOfType<Entity>();
		audios = GetComponent<AudioSource>();
	}

	protected override void Update() {
		base.Update();
		raycast = Physics2D.Raycast((Vector2)transform.position + new Vector2(transform.localScale.x, 0), direction, 3);
		Debug.DrawRay((Vector2)transform.position + new Vector2(1, 0), direction * 3, Color.yellow);

		if (raycast) {
			if (raycast.collider.CompareTag("Entity")) {
				Entity entity = raycast.collider.GetComponent<Entity>();
				if (entity != null && entity != this && entity.faction != faction)
					player = entity;
			}
		}

		if (player != null) {
			if (player.transform.position.x - transform.position.x > 1.5f) x = 1;
			else if (player.transform.position.x - transform.position.x < -1.5f) x = -1;
			else {
				x = 0;
				if (Vector2.Distance(transform.position, player.transform.position) <= 2)
					OnAttackInputUp?.Invoke();
			}
		}

		if (x >= -0.5f && x <= 0.5f)
			anim.SetBool("Run", false);
	}

	public override void AxisInput() {
		base.AxisInput();
		if (onGround)
			anim.SetBool("Run", true);
	}
	public override IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false) {
		if(faction == this.faction) yield break;
		StartCoroutine(base.Damage(faction, damage, knockback, knockbackDirection, hitbox));
		Instantiate(Blood, transform.position, Quaternion.identity);
		yield return null;
	}
	/*public override void Hurt(float damage, float knockback, Vector2 Position) {
		base.Hurt(damage, knockback, Position);

		if (audios != null)
			audios.Play();
		health -= damage;
		Instantiate(Blood, transform.position, Quaternion.identity);

		/*if (health <= 0) {
			if (isPlayer) {
				switch (Random.Range(0, 2)) {
					case 0:
						int j = Random.Range(0, 3);
						for (int i = 0; i < j; i++) {
							Instantiate(Coin, transform.position, Quaternion.identity);
						}
						break;
					case 1:
						int k = Random.Range(0, 2);
						for (int i = 0; i < k; i++) {
							Instantiate(Heart, transform.position, Quaternion.identity);
						}
						break;
					default:
						break;
				}

				/*if (!profile.AttackedTroll && Team == 0)
					profile.TrollKilled++;
				else if (Team == 1) {
					profile.CakeKilled++;
				}
			}

			Destroy(gameObject);
		}
	}*/
}
