using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour {

	public static ItemLibrary instance;
	[SerializeField] public List<Character> characters = new List<Character>();
	[SerializeField] public List<Weapon> weapons = new List<Weapon>();
	[SerializeField] public List<Headgear> headgears = new List<Headgear>();
	[SerializeField] public List<Item> apparels = new List<Item>();
	[SerializeField] public List<Item> accessories = new List<Item>();
	[SerializeField] public List<Utility> utilities = new List<Utility>();

	[SerializeField] private List<Weapon> defaultWeapons = new List<Weapon>();
	public List<Sprite> heroClassIcons;
	public List<Sprite> elementIcons;
	public List<Sprite> limitBreakIcons;
	public List<Sprite> equipIcons;
	public List<Sprite> slotRarities;

	public Dictionary<string, Item> items = new Dictionary<string, Item>();

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else
			Destroy(gameObject);

		//Items
		foreach (Item character in characters) {
			items.Add(character.Name, character);
		}
		foreach (Item weapon in weapons) {
			items.Add(weapon.Name, weapon);
		}
		foreach (Item headgear in headgears) {
			items.Add(headgear.Name, headgear);
		}
		foreach (Item utility in utilities) {
			items.Add(utility.Name, utility);
		}

		foreach (Item weapon in defaultWeapons) {
			items.Add(weapon.Name, weapon);
		}
	}
}
