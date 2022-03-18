using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour {

	[SerializeField] private GameObject FlameOrb, FrostOrb, DarkOrb, GigaOrb;

	private Player Core;
	private Transform LaunchPosition;
	private Projectile projectile;
	private bool Cooldown;

	private void Awake() {
		Core = GetComponent<Player>();
		Core.OnAttackInputUp += AttackUp;

		LaunchPosition = transform.Find("LaunchPosition");
	}

	private void AttackUp() {
		if (!Cooldown) {
			StartCoroutine(AttackUpCoroutine());
		}
	}
	private IEnumerator AttackUpCoroutine() {
		Cooldown = true;
		Core.anim.Play("Attack");
		Core.anim.Play("Arm_Attack");
		yield return new WaitForSeconds(0.083f);
		ProjectileSelection();
		//yield return new WaitForSeconds (Core.Profile.Weapon.Recoil);
		Cooldown = false;
	}

	private void ProjectileSelection() {
		if (Core.anim.GetFloat("Weapon") == 0) {//shoot fire
			projectile = Instantiate(FlameOrb, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile>();
		} else if (Core.anim.GetFloat("Weapon") == 1) {//shoot ice
			projectile = Instantiate(FrostOrb, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile>();
		} else if (Core.anim.GetFloat("Weapon") == 2) {//shoot dark
			projectile = Instantiate(DarkOrb, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile>();
		} else if (Core.anim.GetFloat("Weapon") == 3) {//shoot giga
			projectile = Instantiate(GigaOrb, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile>();
		}
		projectile.SetupProjectileLaunch(Core.direction.x, 0);
		//projectile.Damage += Core.Profile.TrueDamage;
	}
}
