using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Artillery
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
        ammunition.transform.position = new Vector2(transform.position.x + transform.localScale.x, transform.position.y + 0.25f);
        ammunition.ConvertToProjectile(transform.localScale.x * horizontalForce, verticalForce);
        anim.Play("Mortar_Launch");

        //Instantiate(FindObjectOfType<LevelLibrary>().explosionParticle_32x32, new Vector2(transform.position.x + transform.localScale.x, transform.position.y + 0.25f), Quaternion.identity);
    }
}