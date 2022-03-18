using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

	public Item ItemData;
	//private Shop shop;
	//private Profile Profile;
	private ItemLibrary Library;

	private void Awake(){
		//profile=profile.instance;
		Library = ItemLibrary.instance;
	}

	/*public void OnClick(){
		if (Data != null) {
			if (!Inventory) {
				if (!SoldOut) {
					shop.Name.text = Data.Name;
					shop.Description.text = Data.Description;

					shop.BuyButton.onClick.RemoveAllListeners ();
					shop.BuyButton.onClick.AddListener (BuyItem);
					shop.ButtonText.text = "Buy";
					shop.BuyButton.gameObject.SetActive (true);
				}
			} else {
				Profile.Name.text = Data.Name;
				Profile.Description.text = Data.Description;

				Profile.EquipButton.onClick.RemoveAllListeners ();
				if (Data == Profile.Weapon || Data == Profile.Apparel || Data == Profile.Accessory || Data == Profile.CurrentProjectile) {
					bool IsBaseEquipment = false;
					for (int i = 0; i < 4; i++) {
						if (Data == Profile.BaseWeapons [i] || Data == Profile.BaseProjectiles [i])
							IsBaseEquipment = true;
					}
					if (!IsBaseEquipment) {
						Profile.ButtonText.text = "Unequip";
						Profile.EquipButton.onClick.AddListener (UnEquipItem);
						Profile.EquipButton.gameObject.SetActive (true);
					} else {
						Profile.EquipButton.gameObject.SetActive (false);
					}
				} else {
					Profile.ButtonText.text = "Equip";
					Profile.EquipButton.onClick.AddListener (EquipItem);
					Profile.EquipButton.gameObject.SetActive (true);
				}
			}
		}
	}*/

	/*public void SetupItemSlot(Item data, bool inventory){
		Data = data;
		Inventory = inventory;
		transform.GetChild (0).GetComponent<Image> ().sprite = Data.ItemSprite;
	}*/

	//public void BuyItem(){
	//if (Profile.Money >= Data.Price) {
	//if (!SoldOut) {
	//Profile.Money -= Data.Price;
	//shop.WarningPanel.SetActive (true);
	//shop.WarningText.text = "You paid " + Data.Price + "G instead of " + Data.FakePrice + "G";
	/*switch (Data.ItemType) {
	case Item.ItemTypes.Weapon:
		Profile.AddItem (Data, 0, false);
		SoldOut = true;
		transform.GetChild (1).gameObject.SetActive (true);
		shop.BuyButton.gameObject.SetActive (false);
		break;
	case Item.ItemTypes.Apparel:
		Profile.AddItem (Data, 1, false);
		SoldOut = true;
		transform.GetChild (1).gameObject.SetActive (true);
		shop.BuyButton.gameObject.SetActive (false);
		break;
	case Item.ItemTypes.Accessory:
		Profile.AddItem (Data, 2, false);
		SoldOut = true;
		transform.GetChild (1).gameObject.SetActive (true);
		shop.BuyButton.gameObject.SetActive (false);
		break;
	case Item.ItemTypes.Projectile:
		if (Profile.ProjectileCount [Data.ProjectileID] <= 0)
			Profile.AddItem (Data, 0, true);
		Profile.ProjectileCount [Data.ProjectileID] += 10;
		break;
	default:
		break;
	}*/
	//}
	//} else {
	//shop.WarningPanel.SetActive (true);
	//shop.WarningText.text = "You don't have enough money!";
	//}
	//}

	//public void EquipItem(){
	/*switch (Data.ItemType) {
	case Item.ItemTypes.Weapon:
		Profile.Weapon = Data;
		Profile.player.anim.SetFloat("Weapon", Data.Blend);
		break;
	case Item.ItemTypes.Apparel:
		Profile.Apparel = Data;
		Profile.player.Apparel.sprite = Data.ApparelSprite;
		break;
	case Item.ItemTypes.Accessory:
		Profile.Accessory = Data;
		break;
	case Item.ItemTypes.Projectile:
		Profile.CurrentProjectile = Data;
		break;
	default:
		break;
	}*/

	/*bool IsBaseEquipment = false;
	for (int i = 0; i < 4; i++) {
		if (Data == Profile.BaseWeapons [i] || Data == Profile.BaseProjectiles [i])
			IsBaseEquipment = true;
	}
	if (!IsBaseEquipment) {
		Profile.ButtonText.text = "Unequip";
		Profile.EquipButton.onClick.RemoveAllListeners ();
		Profile.EquipButton.onClick.AddListener (UnEquipItem);
	} else {
		Profile.EquipButton.gameObject.SetActive (false);
	}
}*/

	//public void UnEquipItem(){
	/*switch (Data.ItemType) {
	case Item.ItemTypes.Weapon:
		Profile.Weapon = Profile.BaseWeapons [Profile.ClassTypeToInt];
		Profile.player.anim.SetFloat("Weapon", 0);
		break;
	case Item.ItemTypes.Apparel:
		Profile.Apparel = null;
		Profile.player.Apparel.sprite = null;
		break;
	case Item.ItemTypes.Accessory:
		Profile.Accessory = null;
		break;
	case Item.ItemTypes.Projectile:
		if (Profile.BaseWeapons [Profile.ClassTypeToInt] != null) {
			Profile.CurrentProjectile = Profile.BaseWeapons [Profile.ClassTypeToInt];
		}
		break;
	default:
		break;
	}*/
	/*Profile.ButtonText.text = "Equip";
	Profile.EquipButton.onClick.RemoveAllListeners ();
	Profile.EquipButton.onClick.AddListener (EquipItem);
}*/
}
