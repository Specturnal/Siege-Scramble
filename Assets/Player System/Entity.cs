using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, IDamageable {
	public Character character;
	public Factions faction;
	public float maxHealth;
	public float health, attack, defense, speed;//view only
	public Hitbox[] hitboxes;
	public Blockbox[] blockboxes;

	[HideInInspector] public float x, y;//magnitude of directional controls
	//[HideInInspector] public const float minAxisThreshold = 0.5f;
	[HideInInspector] public Vector2 direction;//directional sign
	[HideInInspector] public bool onGround, damaged, shieldUp;
	[HideInInspector] public int jumpCount;

	private Vector3 localScale = Vector3.one;
	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public Animator anim;
	private GameObject flicker;

	[HideInInspector] public bool ignoreInput, ignoreMovement, ignoreMoveset;
	private Coroutine InputCooldownCoroutine, MovementCooldownCoroutine, MovesetCooldownCoroutine;
	public event System.Action OnInputCooldownEnd, OnInputCounterReset, OnMovementCooldownEnd, OnMovementCounterReset, OnMovesetCooldownEnd, OnMovesetCounterReset;

	public event System.Action OnAxisInput;
	public virtual event System.Action OnJumpInputDown;
	public virtual event System.Action OnAttackInputDown, OnAttackInputHold, OnAttackInputUp;
	public virtual event System.Action OnBlockInputDown, OnBlockInputUp;
	public ActionStates ActionState;
	public ActiveStates ActiveState;
	public PassiveStates PassiveState;

	private List<Transform> ladders = new List<Transform>();

	public RaycastHit2D itemFound;
	[HideInInspector] public Carryable carryable;

	protected virtual void Awake() {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

		//Cam = FindObjectOfType<CameraController> ();

		direction = Vector2.right;
		foreach (Hitbox hitbox in hitboxes)
			hitbox.faction = faction;
		flicker = transform.Find("Flicker").gameObject;
	}

	protected virtual void Update() {
		
	}

	protected virtual void FixedUpdate() {
		itemFound = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, -0.25f) + direction * 0.5f, direction, 0.5f);
		Debug.DrawRay((Vector2)transform.position + new Vector2(0, -0.25f) + direction * 0.5f, direction * 0.5f, Color.red);

		switch (ActiveState) {
			case ActiveStates.Default:
				break;
			case ActiveStates.Carrying:
				break;
		}

		switch (PassiveState) {
			case PassiveStates.Default:
				UpdateGravityScale();
				break;
			case PassiveStates.Climbing:
				
				break;
		}
		OnAxisInput?.Invoke(); //must be called at all times
	}

	public virtual void AxisInput() {
		switch (PassiveState) {
			case PassiveStates.Default:
				if (damaged || ignoreMovement) break;
				if (Mathf.Abs(x) > 0.5f) {
					rb.AddForce(Vector2.right * (speed * x));
					float limit = onGround ? speed * 0.5f : speed * 0.25f;
					rb.velocity = Vector2.ClampMagnitude(rb.velocity, limit);
					UpdateDirection();
				} else if (onGround) rb.velocity = new Vector2(0, rb.velocity.y);
				break;
			case PassiveStates.Climbing:
				if (damaged || ignoreMovement) break;
				rb.velocity = new Vector2(Mathf.Abs(x) > 0.5f ? x * 4f : 0, Mathf.Abs(y) > 0.5f ? y * 4f : 0);
				UpdateDirection();
				break;
			case PassiveStates.Swimming:
				break;
		}
	}
	public virtual void JumpInputDown() {
		switch (PassiveState) {
			case PassiveStates.Default:
				if (damaged || ignoreMovement || jumpCount >= 3) break;
				rb.velocity = new Vector2(rb.velocity.x, 0);
				rb.AddForce(Vector2.up * 600);
				jumpCount++;
				break;
			case PassiveStates.Climbing:
				if (damaged || ignoreMovement) break;
				PassiveState = PassiveStates.Default;
				JumpInputDown();
				break;
		}
	}
	private void UpdateDirection() {
		if (Mathf.Abs(x) <= 0.5f || transform.localScale.x == Mathf.Sign(x)) return;
		localScale.Set(Mathf.Sign(x), 1, 1);
		direction = Vector2.right * Mathf.Sign(x);
		transform.localScale = localScale;
		rb.velocity = new Vector2(0, rb.velocity.y);//for instant directional change
	}
	private void UpdateGravityScale() {
		if (onGround && rb.gravityScale != 1) {
			rb.gravityScale = 1;
		} else {
			if (rb.velocity.y > 0) {
				rb.gravityScale = 1;
			} else {
				rb.gravityScale = 1.25f;
			}
		}
	}

	public virtual IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false) {
		if (faction == this.faction) yield break;

		//rmb apply defence formula
		yield return null;
		damaged = true;

		flicker.SetActive(true);
		if (blockboxes.Length >= 1) {
			foreach (Blockbox blockbox in blockboxes) {
				if (blockbox.hitbox != null && blockbox.hitbox == hitbox) {
					blockbox.hitbox = null;
					//dmg
					damage /= 2;
					damaged = false;
					break;
				}
			}
		}
		//hp -= dmg

		rb.AddForce(knockbackDirection * knockback * 20);
		//Invoke("ResetHurt", knockback / 150);
	
		yield return new WaitForSeconds(0.1f);
		flicker.SetActive(false);

		yield return new WaitForSeconds(knockback / 150 - 0.1f);
		damaged = false;
	}

	#region Trigger&Collision
	protected virtual void OnTriggerStay2D(Collider2D other) {
		if (other.CompareTag("Interactable")) {
			//switch(other.)
			//assume it's ladder
			if (!onGround && !ladders.Contains(other.transform) && Mathf.Abs(y) > 0.5f) {
				ladders.Add(other.transform);
				rb.velocity = Vector2.zero;
				rb.gravityScale = 0;
				PassiveState = PassiveStates.Climbing;
			} else if (onGround) {
				PassiveState = PassiveStates.Default;
				rb.gravityScale = 1;
			}
		}
	}
	protected virtual void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Interactable")) {
			if (ladders.Contains(other.transform)) {
				ladders.Remove(other.transform);
			}
			if (ladders.Count == 0) {
				PassiveState = PassiveStates.Default;
				rb.gravityScale = 1;
			}
		}
	}
	protected virtual void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Interactable") || other.gameObject.CompareTag("Entity"))
			jumpCount = 0;
	}
	protected virtual void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Interactable") || other.gameObject.CompareTag("Entity")) {
			if (other.GetContact(0).point.y < transform.position.y - 0.4f)
				onGround = true;
		}
		if (other.gameObject.CompareTag("Interactable")) {
			//switch
			//if(Mathf.Abs(x) > 0.5f && )
		}
	}
	protected virtual void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Interactable") || other.gameObject.CompareTag("Entity"))
			onGround = false;
	}
	#endregion

	#region Cooldown
	public void TriggerCooldown(CooldownTypes cooldownType, WaitForSeconds cooldownDuration, WaitForSeconds counterResetDelay) {
		switch (cooldownType) {
			case CooldownTypes.Input:
				if (InputCooldownCoroutine != null) StopCoroutine(InputCooldownCoroutine);
				InputCooldownCoroutine = StartCoroutine(InputEnumerator(cooldownDuration, counterResetDelay));
				break;
			case CooldownTypes.Movement:
				if (MovementCooldownCoroutine != null) StopCoroutine(MovementCooldownCoroutine);
				MovementCooldownCoroutine = StartCoroutine(MovementEnumerator(cooldownDuration, counterResetDelay));
				break;
			case CooldownTypes.Moveset:
				if (MovesetCooldownCoroutine != null) StopCoroutine(MovesetCooldownCoroutine);
				MovesetCooldownCoroutine = StartCoroutine(MovesetEnumerator(cooldownDuration, counterResetDelay));
				break;
		}
	}
	private IEnumerator InputEnumerator(WaitForSeconds cooldownDelay, WaitForSeconds resetDelay) {
		ignoreInput = true;
		yield return cooldownDelay;
		ignoreInput = false;
		OnInputCooldownEnd?.Invoke();
		OnInputCooldownEnd = null;
		yield return resetDelay;
		OnInputCounterReset?.Invoke();
		OnInputCounterReset = null;
	}
	private IEnumerator MovementEnumerator(WaitForSeconds cooldownDelay, WaitForSeconds resetDelay) {
		ignoreMovement = true;
		yield return cooldownDelay;
		ignoreMovement = false;
		OnMovementCooldownEnd?.Invoke();
		OnMovementCooldownEnd = null;
		yield return resetDelay;
		OnMovementCounterReset?.Invoke();
		OnMovementCounterReset = null;
	}
	private IEnumerator MovesetEnumerator(WaitForSeconds cooldownDelay, WaitForSeconds resetDelay) {
		ignoreMoveset = true;
		yield return cooldownDelay;
		ignoreMoveset = false;
		OnMovesetCooldownEnd?.Invoke();
		OnMovesetCooldownEnd = null;
		yield return resetDelay;
		OnMovesetCounterReset?.Invoke();
		OnMovesetCounterReset = null;
	}
	public enum CooldownTypes {
		Input,
		Movement,
		Moveset
	}
	#endregion

	public enum ActionStates { //For AI prediction
		Idling,
		Running,
		Attacking,
		Blocking
	}
	public enum ActiveStates { //For changing character's respond to controls
		Default,
		Carrying,
		Mounting
	}
	public enum PassiveStates { //For minor behavior changes
		Default,
		Climbing,
		Swimming
	}
}