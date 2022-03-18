using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Hurtbox : MonoBehaviour, IDamageable
{
	[SerializeField] private Factions faction = Factions.Default;
	[SerializeField] private float maxHealth, damageThreshold;
	[SerializeField] private bool destroyObject = true;
	[Tooltip("Add 1 extra frame for ")]
	[SerializeField] private Sprite damagedSprite;
	//[SerializeField] private Sprite[] damagedSprites;
	private float health;

	//public event System.Action<Vector2> OnTakeDamage, OnZeroHealth;
	public UnityEvent<Vector2> OnTakeDamage, OnZeroHealth;

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private Carryable carryable;

	private GameObject flicker;

	private void Awake() {
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		carryable = GetComponent<Carryable>();

		health = maxHealth;
		flicker = transform.Find("Flicker").gameObject;
	}

	public IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false) {
		if (faction == this.faction) yield break;
		if (carryable != null && carryable.joint != null) yield break;

		//rmb apply defence formula for objects?
		yield return null;
		if (rb != null) rb.AddForce(knockbackDirection * knockback * 20);

		if (damage < damageThreshold) yield break;
		health -= damage;
		if (flicker != null) flicker.SetActive(true);
		OnTakeDamage?.Invoke(knockbackDirection);

		if (flicker != null) {
			yield return new WaitForSeconds(0.05f);
			flicker.SetActive(false);
		}

		if (health > 0) yield break;
		health = 0;
		OnZeroHealth?.Invoke(knockbackDirection);
		if (destroyObject) {
			Destroy(gameObject);
		} else {
			sr.sortingLayerName = "Decoration";
			if (damagedSprite != null) sr.sprite = damagedSprite;
			if (carryable != null) Destroy(carryable);
			Destroy(this, 0.1f);
		}

		/*if (damagedSprites.Length > 1) {
			int damageState = destroyObject ? Mathf.CeilToInt((1 - (health / maxHealth)) * (damagedSprites.Length - 1)) : 
				Mathf.FloorToInt((1 - (health / maxHealth)) * (damagedSprites.Length - 1));
			sr.sprite = damagedSprites[damageState];
		}*/
		//Invoke("ResetHurt", knockback / 150);
	}

	private void OnDestroy() {
		
	}
}

public interface IDamageable {
	IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false);
}