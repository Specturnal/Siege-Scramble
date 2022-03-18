using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musketeer : MonoBehaviour {

	[SerializeField] private GameObject Ammo, Golfball, Chicken;

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
		/*if (Core.Profile.CurrentProjectile.ProjectileID == 0) {//shoot arrow
			projectile = Instantiate (Ammo, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
		} else if (Core.Profile.CurrentProjectile.ProjectileID == 3) {//shoot bone
			projectile = Instantiate (Golfball, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
			Core.Profile.ProjectileCount [3]--;
			Core.Profile.ProjectileCountCheck (3);
		} else if (Core.Profile.CurrentProjectile.ProjectileID == 4) {//shoot plungers
			projectile = Instantiate (Chicken, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
			Core.Profile.ProjectileCount [4]--;
			Core.Profile.ProjectileCountCheck (4);
		}*/
		projectile.SetupProjectileLaunch(Core.direction.x, 0);
		//projectile.Damage += (Core.Profile.TrueDamage);
	}
}
