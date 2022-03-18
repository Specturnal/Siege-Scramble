using UnityEngine;

[CreateAssetMenu(menuName = "Item/Utility", order = 5)]
public class Utility : Item
{
    [Header("Utility")]
    public HeroClassTypes HeroClass;
    public Sprite Appearance;
    public AnimationClip[] animations;
}
