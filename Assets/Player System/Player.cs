using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;

public class Player : Entity {

	[SerializeField] private bool mobile;
	//[SerializeField]private AudioClip CoinPickupSFX;

	private AudioSource audioS;

	private Vector2 UpwardsKnockback = new Vector2(0, 2);

	public Profile Profile;
	//private CameraController Cam;

	public bool attackButtonDown, blockButtonDown;
	 
	public override event System.Action OnJumpInputDown;
	public override event System.Action OnAttackInputDown, OnAttackInputHold, OnAttackInputUp;
	public override event System.Action OnBlockInputDown, OnBlockInputUp;

	public TouchScreenInput screenInput;
	//public InputAction controls;

	protected override void Awake() {
		base.Awake();
		audioS = GetComponent<AudioSource>();

		//Cam = FindObjectOfType<CameraController> ();

		//Profile.hp = HP;
	}

	private void Start() {
		screenInput = FindObjectOfType<TouchScreenInput>();
	}

	protected override void Update() {
		base.Update();
		if (ignoreInput) return;

		DetectMovement();
		DetectMoveset();
	}

	protected override void FixedUpdate() {
		if (onGround)
			anim.SetInteger("Airborne", 0);
		else {
			if (rb.velocity.y > 0)
				anim.SetInteger("Airborne", 2);
			else
				anim.SetInteger("Airborne", 1);
		}
		base.FixedUpdate();
	}

	private void DetectMovement() {
		x = Input.GetAxisRaw("Horizontal");
		y = Input.GetAxisRaw("Vertical");
		if (mobile) {
			x = screenInput.x;
			y = screenInput.y;
		}

		switch (PassiveState) {
			case PassiveStates.Default:
				if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical") && y > 0.5f) OnJumpInputDown?.Invoke();
				break;
			case PassiveStates.Climbing:
				if (Input.GetButtonDown("Jump")) OnJumpInputDown?.Invoke();
				break;
		}
	}
	private void DetectMoveset() {
		if (!blockButtonDown) {//AttackButton
			if (Input.GetButtonDown("Fire1")) {
				OnAttackInputDown?.Invoke();
				attackButtonDown = true;
			}
			if (Input.GetButton("Fire1")) {
				OnAttackInputHold?.Invoke();
			}
			if (Input.GetButtonUp("Fire1")) {
				OnAttackInputUp?.Invoke();
				attackButtonDown = false;
			}
		}
		if (!attackButtonDown) {//BlockButton
			if (Input.GetButtonDown("Fire2")) {
				OnBlockInputDown?.Invoke();
				blockButtonDown = true;
			}
			if (Input.GetButton("Fire2")) { 
			
			}
			if (Input.GetButtonUp("Fire2")) {
				OnBlockInputUp?.Invoke();
				blockButtonDown = false;
			}
		}
	}
	public override void AxisInput() {
		base.AxisInput();
		switch (PassiveState) {
			case PassiveStates.Default:
				anim.SetBool("Run", !damaged && !ignoreMovement && Mathf.Abs(x) > 0.5f);
				break;
			case PassiveStates.Climbing:
				if (damaged || ignoreMovement) break;

				break;
		}
	}

	public override IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false){
		StartCoroutine(base.Damage(faction, damage, knockback, knockbackDirection, hitbox));
		yield break;
		/*if (Profile.TrueDefense >= 1)
			Profile.hp -= Damage / Profile.TrueDefense;
		else
			Profile.hp -= Damage;
		IgnoreInput = true;
		rb.AddForce ((
			((Vector2)transform.position - Position) / Vector2.Distance (Position, transform.position) + UpwardsKnockback
		) * Knockback);
		CancelInvoke ("ResetHurt");
		Invoke ("ResetHurt", 0.1f);

		if (Profile.hp <= 0) {
			Destroy (gameObject);
		}*/
	}

	protected override void OnCollisionEnter2D(Collision2D other) {
		base.OnCollisionEnter2D(other);
		/*if (other.gameObject.CompareTag ("Coin")) {
			Profile.Money += 500;
			audioS.Stop ();
			audioS.clip = CoinPickupSFX;
			audioS.Play ();
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag ("Heart")) {
			Profile.hp += 10;
			Destroy (other.gameObject);
		}*/
	}
}
