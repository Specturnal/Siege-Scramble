using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour {

	/*archer
	 * private void Awake(){
		Core = GetComponent<Player> ();
		Core.AttackDown += AttackDown;
		Core.AttackUp += AttackUp;

		LaunchPosition = transform.Find ("LaunchPosition");
	}

	private void AttackDown(){
		if (!Cooldown && !Core.anim.GetBool("Hold")) {
			Core.anim.SetBool ("Hold", true);
			Core.anim.Play ("Attack");
			Core.anim.Play("Arm_Attack");
		}
	}
	private void AttackUp(){
		if (!Cooldown && Core.anim.GetBool ("Hold")) {
			StopCoroutine (AttackUpCoroutine ());
			StartCoroutine (AttackUpCoroutine ());
		}
	}
	private IEnumerator AttackUpCoroutine(){
		Cooldown = true;
		Core.anim.SetBool ("Hold", false);
		ProjectileSelection ();
		yield return new WaitForSeconds (Core.Profile.Weapon.Recoil);
		Cooldown = false;
	}

	private void ProjectileSelection(){
		if (Core.Profile.CurrentProjectile.ProjectileID == 0) {//shoot arrow
			projectile = Instantiate (Arrow, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
		} else if (Core.Profile.CurrentProjectile.ProjectileID == 1) {//shoot bone
			projectile = Instantiate (CursedArrow, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
			Core.Profile.ProjectileCount [1]--;
			Core.Profile.ProjectileCountCheck (1);
		} else if (Core.Profile.CurrentProjectile.ProjectileID == 2) {//shoot plungers
			projectile = Instantiate (PiercingVoid, LaunchPosition.position, Quaternion.identity).GetComponent<Projectile> ();
			Core.Profile.ProjectileCount [2]--;
			Core.Profile.ProjectileCountCheck (2);
		}
		projectile.SetupProjectile (Core.Direction.x, 0);
		projectile.Damage += Core.Profile.TrueDamage;
	}*/
}
