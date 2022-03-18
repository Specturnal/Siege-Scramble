using UnityEngine;

public class Knight : MonoBehaviour {

	public GameObject swordBeam;

	private Entity entity;

	private int N_AtkState, A_AtkState;
	private readonly WaitForSeconds N_AtkDuration = new WaitForSeconds(0.25f), A_AtkDuration = new WaitForSeconds(0.3f);
	private float chargeTimer;

	[Range(0, 100)] public float knockback;

	private void Awake() {
		entity = GetComponent<Entity>();

		entity.OnAxisInput += entity.AxisInput;
		entity.OnJumpInputDown += entity.JumpInputDown;

		entity.OnAttackInputHold += AttackInputHold;
		entity.OnAttackInputUp += AttackInputUp;
		entity.OnBlockInputDown += BlockInputDown;
		entity.OnBlockInputUp += BlockInputUp;
	}

	private void AttackInputHold() {
		if (entity.ActiveState == Entity.ActiveStates.Carrying) return;

		if (entity.ignoreMoveset || !entity.onGround || chargeTimer >= 0.75f) return;

		chargeTimer += Time.deltaTime;
		entity.ignoreMovement = true;
		if (chargeTimer >= 0.2f)
			entity.anim.SetBool("Charge", true);
		if (chargeTimer >= 0.75f) {
			//Charge
			Projectile SwordBeam_0 = Instantiate(swordBeam, transform.position + new Vector3(entity.direction.x * 1.25f, 0.5f, 0), Quaternion.identity).GetComponent<Projectile>();
			SwordBeam_0.SetupHitbox(entity.faction, 1, knockback / 1.5f, entity.direction.x, 0.5f);
			SwordBeam_0.SetupProjectileLaunch(entity.direction.x, 0, launchDelay: 0.25f);
			Projectile SwordBeam_1 = Instantiate(swordBeam, transform.position + new Vector3(entity.direction.x * 1.25f, 0.25f, 0), Quaternion.identity).GetComponent<Projectile>();
			SwordBeam_1.SetupHitbox(entity.faction, 1, knockback / 1.5f, entity.direction.x, 0.5f);
			SwordBeam_1.SetupProjectileLaunch(entity.direction.x, 0, launchDelay: 0.5f);
			Projectile SwordBeam_2 = Instantiate(swordBeam, transform.position + new Vector3(entity.direction.x * 1.25f, 0, 0), Quaternion.identity).GetComponent<Projectile>();
			SwordBeam_2.SetupHitbox(entity.faction, 1, knockback / 1.5f, entity.direction.x, 0.5f);
			SwordBeam_2.SetupProjectileLaunch(entity.direction.x, 0, launchDelay: 0.75f);

			entity.TriggerCooldown(Entity.CooldownTypes.Movement, new WaitForSeconds(0.5f), new WaitForSeconds(0));
			entity.TriggerCooldown(Entity.CooldownTypes.Moveset, new WaitForSeconds(0.5f), new WaitForSeconds(0));
			entity.OnMovesetCooldownEnd += () => {
				entity.anim.SetBool("Charge", false);
				entity.anim.Play("Knight_Discharge-0");
			};
		}
	}
	private void AttackInputUp() {
		if (entity.ActiveState == Entity.ActiveStates.Carrying) return;

		if (!entity.ignoreMoveset) {
			 if (chargeTimer < 1f && (!entity.onGround || entity.y > 0)) {
				//Aerial
				A_AtkState++;
				if (A_AtkState == 1) {
					entity.hitboxes[0].SetupHitbox(4, knockback, entity.direction.x / 4, 1f);
					entity.anim.Play("Knight_Aerial-0");
					entity.JumpInputDown();
				} else {
					entity.hitboxes[0].SetupHitbox(4, knockback, entity.direction.x, -0.5f);
					entity.anim.Play("Knight_Neutral-1");
				}
				entity.TriggerCooldown(Entity.CooldownTypes.Moveset, A_AtkDuration, A_AtkDuration);
				entity.OnMovesetCounterReset += () => A_AtkState = 0;
			}else if (entity.onGround && chargeTimer < 1f) {
				//Neutral
				N_AtkState++;
				if (N_AtkState % 2 == 1) {
					entity.hitboxes[0].SetupHitbox(6, knockback, entity.direction.x, 0.5f);
					entity.anim.Play("Knight_Neutral-0");
				} else {
					entity.hitboxes[0].SetupHitbox(6, knockback, entity.direction.x, -0.5f);
					entity.anim.Play("Knight_Neutral-1");
				}
				entity.TriggerCooldown(Entity.CooldownTypes.Moveset, N_AtkDuration, N_AtkDuration);
				entity.OnMovesetCounterReset += () => N_AtkState = 0;
			}
			entity.ignoreMovement = false;
			entity.anim.SetBool("Charge", false);
		}
		chargeTimer = 0;
	}

	private void BlockInputDown() {
		switch (entity.ActiveState) {
			case Entity.ActiveStates.Default:
				if (entity.ignoreMoveset) break;

				if (Mathf.Abs(entity.x) > 0.5f && entity.itemFound && entity.itemFound.collider.CompareTag("Interactable")) {
					if (entity.itemFound.collider.TryGetComponent(out entity.carryable)) {
						//Carry Object
						entity.carryable.CarryObject(entity.rb, transform.Find("Item"));
						entity.anim.SetBool("Carry", true);
						entity.ActiveState = Entity.ActiveStates.Carrying;
						entity.TriggerCooldown(Entity.CooldownTypes.Movement, new WaitForSeconds(0.4f), new WaitForSeconds(0));
						entity.TriggerCooldown(Entity.CooldownTypes.Moveset, new WaitForSeconds(0.4f), new WaitForSeconds(0));
						return;
					}
				}
				//Block
				entity.anim.SetBool("Block", true);
				entity.ignoreMovement = true;
				break;
			case Entity.ActiveStates.Carrying:
				if (entity.ignoreMoveset) break;

				if (Mathf.Abs(entity.x) > 0.5f) {
					//Throw Object
					entity.anim.SetTrigger("Throw");
					entity.carryable.DropObject(throwDirection: entity.direction.x);
					entity.TriggerCooldown(Entity.CooldownTypes.Movement, new WaitForSeconds(1f), new WaitForSeconds(0));
				} else {
					//Drop Object
					entity.carryable.DropObject(dropDirection: entity.direction.x);
					if (entity.itemFound && entity.itemFound.collider.CompareTag("Interactable")) {
						Ammunition ammunition;
						Artillery artillery;
						if (entity.carryable.TryGetComponent(out ammunition) && entity.itemFound.collider.TryGetComponent(out artillery))
							artillery.StartCoroutine(artillery.LoadAmmunition(ammunition));
					}
					entity.TriggerCooldown(Entity.CooldownTypes.Movement, new WaitForSeconds(0.4f), new WaitForSeconds(0));
				}
				entity.anim.SetBool("Carry", false);
				entity.carryable = null;
				entity.ActiveState = Entity.ActiveStates.Default;
				entity.TriggerCooldown(Entity.CooldownTypes.Moveset, new WaitForSeconds(0.4f), new WaitForSeconds(0));
				break;
		}
	}
	private void BlockInputUp() {
		switch (entity.ActiveState) {
			case Entity.ActiveStates.Default:
				entity.anim.SetBool("Block", false);
				entity.ignoreMovement = false;
				break;
		}
	}

	private void UltiDown() { 
	}
}