using UnityEngine;

public class Item : ScriptableObject {

	public int ItemID;
	
	public Sprite ItemSprite;
	[TextArea]public string Name, Description;

    public enum RarityTypes {
        Common,
        Rare,
        SuperRare
    }

	public enum HeroClassTypes
	{
		General,
		Knight,
		Archer,
		Wizard,
		Musketeer
	}

    public enum ElementTypes
    {
        Normal,
        Fire,
        Water,
        Grass,
        Electric,
        Earth,
        Wind,
        Light,
        Dark,
        Void
    }

    public enum CalculationTypes {
        Direct,
        Percentage
    }
}
