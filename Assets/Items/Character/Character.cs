using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Character", order = 0)]
public class Character : Item
{
    [Header("Character")]
    public RarityTypes Rarity;
    public HeroClassTypes[] HeroClasses = new HeroClassTypes[1];
    public int Hp, Atk, Def, Spd;
    public ElementTypes Element;
    public AnimatorOverrideController animator;
    //public Sprite[] leg_0Sprite, leg_1Sprite;
    //public AnimationClip[] leg_0Clip, leg_1Clip;

    [ContextMenu("SetToDefault")]
    private void SetToDefault() {
        Rarity = RarityTypes.Common;
        HeroClasses[0] = HeroClassTypes.General;
        Element = ElementTypes.Normal;
    }
}