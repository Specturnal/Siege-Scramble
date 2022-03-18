using UnityEngine;

[CreateAssetMenu(menuName = "Item/Apparel", order = 3)]
public class Apparel : Item
{
    [Header("Apparel")]
    public RarityTypes Rarity;
    public float Hp;
    public CalculationTypes HpCalculation;
    public float Def { get; set; }
    public CalculationTypes DefCalculation;
    public float Spd;
    public CalculationTypes SpdCalculation;
}
