using UnityEngine;

public class Blockbox : MonoBehaviour
{
	public GameObject hitbox;

	private void OnDisable() {
		hitbox = null;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Hitbox")) {
			hitbox = other.gameObject;
		}
	}
}