using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Artillery
{
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        //GetComponent<Hurtbox>().OnZeroHealth += OnZeroHealth;
        GetComponent<Hurtbox>().OnZeroHealth.AddListener(OnZeroHealth);
    }

    private void OnZeroHealth(Vector2 knockbackDirection) {
        Destroy(anim);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
        Destroy(this);
    }

    public override IEnumerator LoadAmmunition(Ammunition ammunition, float dropAnimationDelay = 0.25f) {
        yield return new WaitForSeconds(dropAnimationDelay);
        anim.Play("Catapult_Launch");
        Carryable carryable = ammunition.GetComponent<Carryable>();
        carryable.CarryObject(GetComponent<Rigidbody2D>(), transform.Find("AmmoPosition"));
        yield return new WaitForSeconds(0.65f);
        carryable.DropObject(animationDuration: 0, transform.localScale.x);
        ammunition.ConvertToProjectile(transform.localScale.x * horizontalForce, verticalForce);

        //Instantiate(FindObjectOfType<LevelLibrary>().explosionParticle_32x32, new Vector2(transform.position.x + transform.localScale.x, transform.position.y + 0.25f), Quaternion.identity);
    }
}
