using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Accessory", order = 4)]
public class Accessory : Item
{
    [Header("Accessory")]
    public MedalSystem[] MedalEffect;
    public UltiSystem[] UltimateEffect;
}

[System.Serializable]
public struct MedalSystem {
    public Stats Stat;
    public float Value;
    public CalculationTypes Calculation;

    public enum Stats {
        HP,
        ATK,
        DEF,
        SPD
    }
    public enum CalculationTypes {
        Direct, 
        Percentage
    }
}

[System.Serializable]
public struct UltiSystem { 

}
