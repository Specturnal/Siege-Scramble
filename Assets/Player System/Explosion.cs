using UnityEngine;

public class Explosion : Hitbox {
	[SerializeField] private ExplosionSizes explosionSize;
	[SerializeField] private float lifeTime;

	private void Start() {
		if (lifeTime != 0) Destroy(gameObject, lifeTime);
		Invoke("SpawnParticle", 0.5f);
	}

	private void SpawnParticle() {
		switch (explosionSize) {
			case ExplosionSizes.explosion32x32:
				Instantiate(FindObjectOfType<LevelLibrary>().explosionParticle_32x32, transform.position, Quaternion.identity);
				break;
			case ExplosionSizes.explosion64x64:
				Instantiate(FindObjectOfType<LevelLibrary>().explosionParticle_64x64, transform.position, Quaternion.identity);
				break;
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Entity") || other.CompareTag("Interactable")) {
			damageable = other.GetComponent<IDamageable>();

			if (damageable != null) {
				knockbackDirection = (other.transform.position - transform.position).normalized;
				if (Physics2D.Raycast(transform.position, knockbackDirection, 2f).collider == other) {
					(damageable as MonoBehaviour).StartCoroutine(damageable.Damage(faction, damage, knockback, knockbackDirection, gameObject));
				}
			}
		}
	}

	public enum ExplosionSizes {
		explosion32x32,
		explosion64x64
	}
}