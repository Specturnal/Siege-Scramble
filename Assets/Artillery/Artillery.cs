using System.Collections;
using UnityEngine;

public abstract class Artillery : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] protected float horizontalForce, verticalForce;
    public abstract IEnumerator LoadAmmunition(Ammunition ammunition, float dropAnimationDelay = 0.25f);
}
