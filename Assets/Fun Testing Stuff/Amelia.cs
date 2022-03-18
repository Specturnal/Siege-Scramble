
using UnityEngine;

public class Amelia : MonoBehaviour {
	[HideInInspector] public float x, y;//magnitude of directional controls
										//[HideInInspector] public const float minAxisThreshold = 0.5f;
	[HideInInspector] public Vector2 direction;//directional sign
	[HideInInspector] public bool onGround, damaged, shieldUp;
	[HideInInspector] public int jumpCount;
	public float health, attack, defense, speed;//view only
	private Vector3 localScale = Vector3.one;
	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public Animator anim;

	[HideInInspector] public bool ignoreInput, ignoreMovement, ignoreMoveset;
	public event System.Action OnAxisInput;
	public virtual event System.Action OnJumpInputDown;

	void Start() {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

		//Cam = FindObjectOfType<CameraController> ();
		OnAxisInput += AxisInput;
		OnJumpInputDown += JumpInputDown;
		direction = Vector2.right;
	}

	void FixedUpdate() {
		OnAxisInput?.Invoke(); //must be called at all times
	}
	private void Update() {
		DetectMovement();
	}

	private void DetectMovement() {
		x = Input.GetAxisRaw("Horizontal");
		y = Input.GetAxisRaw("Vertical");

		if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical") && y > 0.5f) OnJumpInputDown?.Invoke();
	}
	public virtual void AxisInput() {
		anim.SetBool("Run", !damaged && !ignoreMovement && Mathf.Abs(x) > 0.5f);
		if (damaged || ignoreMovement) return;
		if (Mathf.Abs(x) > 0.5f) {
			rb.AddForce(Vector2.right * (speed * x));
			float limit = onGround ? speed * 0.5f : speed * 0.25f;
			rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -limit, limit), rb.velocity.y);
			UpdateDirection();
		} else if (onGround) rb.velocity = new Vector2(0, rb.velocity.y);
	}
	public virtual void JumpInputDown() {
		if (damaged || ignoreMovement || jumpCount > 2) return;
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(Vector2.up * 600);
		anim.Play("Jump");
		jumpCount++;
		return;
	}
	private void UpdateDirection() {
		if (Mathf.Abs(x) <= 0.5f || transform.localScale.x == Mathf.Sign(x)) return;
		localScale.Set(Mathf.Sign(x) * 2.5f, 2.5f, 1);
		direction = Vector2.right * Mathf.Sign(x);
		transform.localScale = localScale;
		rb.velocity = new Vector2(0, rb.velocity.y);//for instant directional change
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
}
