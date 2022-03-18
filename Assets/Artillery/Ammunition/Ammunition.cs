using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour {
	//public Factions faction;
	[SerializeField] protected float damage, knockback;
	[SerializeField] private bool unblockable;

	[SerializeField] private float speed, lifeTime;
	[SerializeField] [Range(0, 100)] private int accuracy = 100;
	[SerializeField] [Tooltip("Destroys gameobject upon contact")] private bool destroyOnContact = true;
	[SerializeField] private Projectile.TraversalMethods traversalMethod;
	[SerializeField] private Carryable.DirectionalOptions directionalOption = Carryable.DirectionalOptions.FaceLaunchDirection;

	public void ConvertToProjectile(float launchDirection_x, float launchDirection_y) {
		Projectile projectile = gameObject.AddComponent<Projectile>();
		projectile.SetupHitbox(Factions.Default, damage, knockback, launchDirection_x, 0.5f, unblockable: unblockable);
		projectile.SetupProjectileProperties(speed, destroyOnContact: destroyOnContact, traversalMethod:traversalMethod, directionalOption: directionalOption);
		projectile.SetupProjectileLaunch(launchDirection_x, launchDirection_y, activeDelay: 0.01f);
		projectile.objectsToLoadOnHit.Add(FindObjectOfType<LevelLibrary>().explosion_32x32);
		Destroy(this);

		GameObject trail = transform.GetChild(0).gameObject;
		if (trail != null) {
			trail.SetActive(true);
		}
	}
}