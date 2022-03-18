using UnityEngine;
using System.Collections;

public class Carryable : MonoBehaviour {
    [SerializeField] private DirectionalOptions directionalOption;
    [SerializeField] private float throwForce = 20;
    [SerializeField] private float throwDamage, throwKnockback;
    public FixedJoint2D joint { get; private set; }
    private Transform refTransform;
    private WaitForEndOfFrame waitForEndOfFrame;
    private float initialMass;

    public void CarryObject(Rigidbody2D connectedBody, Transform refTransform, float animationDuration = 0.5f) {
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = connectedBody;

        initialMass = joint.attachedRigidbody.mass;
        joint.attachedRigidbody.mass = 0.001f;
        joint.enableCollision = false;
        this.refTransform = refTransform;
        StartCoroutine(CarryEnumerator(animationDuration));
    }

    public void DropObject(float animationDuration = 0.25f, float dropDirection = 1, float throwDirection = 0) {
        if (joint != null) {
            joint.attachedRigidbody.mass = initialMass;
            Destroy(joint);
            joint = null;
        }
        if (directionalOption != DirectionalOptions.KeepDefaultDirection) {
            transform.localScale = new Vector2((int)directionalOption * dropDirection, 1);
        }
        
        StartCoroutine(CarryEnumerator(animationDuration));

        if (throwDirection != 0) {
            Projectile projectile = gameObject.AddComponent<Projectile>();
            projectile.SetupHitbox(Factions.Default, throwDamage, throwKnockback, throwDirection, 0.5f);//change vector
            projectile.SetupProjectileProperties(throwForce, destroyOnContact: false, traversalMethod:Projectile.TraversalMethods.UseForce, directionalOption: directionalOption);
            projectile.SetupProjectileLaunch(throwDirection, 0, launchDelay: animationDuration, activeDelay: 0.05f);
        }
    }

    private IEnumerator CarryEnumerator(float animationDuration) {
        do {
            yield return null;
            transform.position = refTransform.position;
            animationDuration -= Time.deltaTime;
        } while (animationDuration > 0);
    }

    private void OnDestroy() {
        if (joint != null) {
            joint.attachedRigidbody.mass = initialMass;
            Destroy(joint);
            joint = null;
        }
    }

    public enum DirectionalOptions {
        KeepDefaultDirection,
        FaceLaunchDirection = 1,
        InverseOfLaunchDirection = -1
    }
}