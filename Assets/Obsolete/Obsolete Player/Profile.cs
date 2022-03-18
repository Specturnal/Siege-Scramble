using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Profile : MonoBehaviour {

	[Header("PlayerInfo")]
	public int Money;

	//static variables
	public static int HeroClassID, SkinID, WeaponID, ApparelID, AccessoryID, ProjectileID;
	public static float Health, Attack, Defense, Speed;
	public static float BaseHP, BaseATK, BaseDEF, BaseSPD;
	public static float SubHP, SubATK, SubDEF, SubSPD;

	public void AssignValue(){
		
	}

	public void SaveLOL(){
		PlayerPrefs.SetInt ("HeroClass", HeroClassID);
		PlayerPrefs.SetInt ("Skin", HeroClassID);
		PlayerPrefs.SetInt ("Weapon", WeaponID);
		PlayerPrefs.SetInt ("Apparel", ApparelID);
		PlayerPrefs.SetInt ("Accessory", AccessoryID);
		PlayerPrefs.SetInt ("Projectile", ProjectileID);
	}

	public void LoadLOL(){
		if (PlayerPrefs.HasKey ("HeroClass"))
			HeroClassID = PlayerPrefs.GetInt ("HeroClass");
		if (PlayerPrefs.HasKey ("Skin"))
			SkinID = PlayerPrefs.GetInt ("Skin");
		if (PlayerPrefs.HasKey ("Weapon"))
			WeaponID = PlayerPrefs.GetInt ("Weapon");
		if (PlayerPrefs.HasKey ("Apparel"))
			ApparelID = PlayerPrefs.GetInt ("Apparel");
		if (PlayerPrefs.HasKey ("Accessory"))
			AccessoryID = PlayerPrefs.GetInt ("Accessory");
		if (PlayerPrefs.HasKey ("Projectile"))
			ProjectileID = PlayerPrefs.GetInt ("Projectile");
	}

	public void CalculateStat(){
		//BaseHP = Library.Character [SkinID].Health + Library.Weapon [WeaponID].Health + Library.Utility [ProjectileID].Health;
		//BaseATK = Library.Character [SkinID].Attack + Library.Weapon [WeaponID].Attack + Library.Utility [ProjectileID].Attack;
		//BaseDEF = Library.Character [SkinID].Defense + Library.Weapon [WeaponID].Defense + Library.Utility [ProjectileID].Defense;
		//BaseSPD = Library.Character [SkinID].Speed + Library.Weapon [WeaponID].Speed + Library.Utility [ProjectileID].Speed;

		//SubHP = Library.Apparel [ApparelID].Health + Library.Accessory [AccessoryID].Health;
		//SubATK = Library.Apparel [ApparelID].Attack + Library.Accessory [AccessoryID].Attack;
		//SubDEF = Library.Apparel [ApparelID].Defense + Library.Accessory [AccessoryID].Defense;
		//SubSPD = Library.Apparel [ApparelID].Speed + Library.Accessory [AccessoryID].Speed;

		Health = BaseHP + SubHP;
		Attack = BaseATK + SubATK;
		Defense = BaseDEF + SubDEF;
		Speed = BaseSPD + SubSPD;
	}
}
