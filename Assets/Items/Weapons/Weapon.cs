using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", order = 1)]
public class Weapon : Item
{
	[Header("Weapon")]
	public RarityTypes Rarity;
	public HeroClassTypes HeroClass;
	public float Atk;
	public CalculationTypes AtkCalculation;
	public float Knockback;
	public float Recoil;
	public AnimationClip[] animations;
}
