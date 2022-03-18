using UnityEngine;

[CreateAssetMenu(menuName = "Item/Headgear", order = 2)]
public class Headgear : Item
{
    [Header("Headgear")]
    public RarityTypes Rarity;
    public HeroClassTypes[] HeroClasses = new HeroClassTypes[1];
    public float Hp;
    public CalculationTypes HpCalculation;
    public float Def;
    public CalculationTypes DefCalculation;
    public float Spd;
    public CalculationTypes SpdCalculation;
    public Sprite spriteMask;
    public bool useMask;
    public AnimationClip animation;

    [ContextMenu("SetToDefault")]
    private void SetToDefault() {
        Rarity = RarityTypes.Common;
        HeroClasses[0] = HeroClassTypes.General;
        useMask = true;
    }
}
