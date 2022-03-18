using UnityEngine;

public class Hitbox : MonoBehaviour {
	public Factions faction;
	[SerializeField] private bool continuousDamage;
	[SerializeField] protected float damage, knockback;
	protected Vector2 knockbackDirection;
	[SerializeField] private bool unblockable;

	protected Entity entity;
	protected IDamageable damageable;

	public void SetupHitbox(float damage, float knockback, float x_knockback, float y_knockback, bool unblockable = false) {
		(this.damage, this.knockback, this.unblockable) = (damage, knockback, unblockable);
		knockbackDirection.Set(x_knockback, y_knockback);
	}
	public void SetupHitbox(Factions faction, float damage, float knockback, float x_knockback, float y_knockback, bool unblockable = false) {
		(this.faction, this.damage, this.knockback, this.unblockable) = (faction, damage, knockback, unblockable);
		knockbackDirection.Set(x_knockback, y_knockback);
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Entity") || other.CompareTag("Interactable")) {
			damageable = other.GetComponent<IDamageable>();

			if (damageable != null) {
				(damageable as MonoBehaviour).StartCoroutine(damageable.Damage(faction, damage, knockback, knockbackDirection, gameObject));
			}
		}
	}
}

public enum Factions {
	Default,//world objects
	NoFaction,//neutral, explosion
	Faction1,
	Faction2,
	Faction3
}