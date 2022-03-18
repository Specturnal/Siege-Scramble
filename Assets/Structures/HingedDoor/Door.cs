using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite broken_L, broken_R;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        //GetComponent<Hurtbox>().OnZeroHealth += OnZeroHealth;
        GetComponent<Hurtbox>().OnZeroHealth.AddListener(OnZeroHealth);
    }

    private void OnZeroHealth(Vector2 knockbackDirection) {
        Destroy(GetComponent<Collider2D>());
        Destroy(anim);
        GetComponent<SpriteRenderer>().sprite = knockbackDirection.x > 0 ? broken_R : broken_L;
        Destroy(this);
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Entity")) {
            anim.SetBool("Opened", true);
            anim.SetBool("R", other.transform.position.x < transform.position.x);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Entity")) {
            anim.SetBool("Opened", false);
        }
    }
}
