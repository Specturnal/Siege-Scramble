using System.Collections;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamageable
{
    [SerializeField] private Factions faction = Factions.Default;
	[SerializeField] private float damageThreshold;
	[SerializeField] private bool destroyObject = true;
	[SerializeField] private Explosion.ExplosionSizes explosionSize;
	//[Tooltip("Add 1 extra frame for ")]
	//[SerializeField] private Sprite[] damagedSprites;

	private SpriteRenderer sr;
	//private Rigidbody2D rb;
	private Carryable carryable;

	private void Awake() {
		sr = GetComponent<SpriteRenderer>();
		//rb = GetComponent<Rigidbody2D>();
		carryable = GetComponent<Carryable>();
	}

	public IEnumerator Damage(Factions faction, float damage, float knockback, Vector2 knockbackDirection, GameObject hitbox, bool unblockable = false) {
		if (faction == this.faction) yield break;
		if (carryable != null && carryable.joint != null) yield break;

		yield return new WaitForEndOfFrame();
		//if (rb != null) rb.AddForce(knockbackDirection * knockback * 20);

		//hurt = true;
		if (damage >= damageThreshold) {
			StartCoroutine(Ignite());
		}
		/*if (damagedSprites.Length > 1) {
			int damageState = destroyObject ? Mathf.CeilToInt((1 - (health / maxHealth)) * (damagedSprites.Length - 1)) :
				Mathf.FloorToInt((1 - (health / maxHealth)) * (damagedSprites.Length - 1));
			sr.sprite = damagedSprites[damageState];
		}*/
		//Invoke("ResetHurt", knockback / 150);
	}

    private IEnumerator Ignite() {
		switch (explosionSize) {
			case Explosion.ExplosionSizes.explosion32x32:
				Instantiate(FindObjectOfType<LevelLibrary>().explosion_32x32, transform.position, Quaternion.identity);
				break;
			case Explosion.ExplosionSizes.explosion64x64:
				Instantiate(FindObjectOfType<LevelLibrary>().explosion_64x64, transform.position, Quaternion.identity);
				break;
		}
		Destroy(gameObject, 0.5f);
        yield return null;
    }
}
