using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
    [Header("Player Combo")]
    public Character character;
    public Weapon weapon;
    public Headgear headgear;
    public Apparel apparel;
    public Accessory[] accessories = new Accessory[3];
    public Utility utility;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerEntity;
    [SerializeField] private AnimationClip Knight_Block_1;
    [SerializeField] private GameObject SwordBeam;

    private Player player;

    private void Awake() {
        
    }

    private void Start() {
        player = Instantiate(playerEntity, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        //player.character = character;
        player.GetComponent<CharacterAnimationSystem>().character = character;
        CalculateHP();
        CalculateATK();
        CalculateDEF();
        CalculateSPD();

        AnimatorOverrideController anim = Instantiate(character.animator);

        if (headgear != null) {
            Transform playerHead = player.transform.Find("Head");
            Transform playerHeadgear = playerHead.transform.GetChild(0);
            playerHeadgear.gameObject.SetActive(true);
            if (headgear.useMask) {
                playerHead.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                playerHeadgear.GetComponent<SpriteMask>().sprite = headgear.spriteMask;
            }
            anim["Template_Headgear_0"] = headgear.animation;
        }

        switch (weapon.HeroClass) {
            case Item.HeroClassTypes.Knight:
                SetupKnight(anim); break;
            case Item.HeroClassTypes.Archer:
                SetupArcher(anim); break;
        }

        player.anim.runtimeAnimatorController = anim;
    }

    private void CalculateHP() {
        player.health = character.Hp;
        if (headgear != null) {
            if (headgear.HpCalculation == Item.CalculationTypes.Direct)
                player.health += headgear.Hp;
            else if (headgear.HpCalculation == Item.CalculationTypes.Percentage)
                player.health += character.Hp * headgear.Hp / 100;
        }
        if (apparel != null) {
            if (apparel.HpCalculation == Item.CalculationTypes.Direct)
                player.health += apparel.Hp;
            else if (apparel.HpCalculation == Item.CalculationTypes.Percentage)
                player.health += character.Hp * apparel.Hp / 100;
        }

        if (player.health < 1)
            player.health = 1;
    }

    private void CalculateATK() {
        player.attack = character.Atk;
        if (weapon != null) {
            if (weapon.AtkCalculation == Item.CalculationTypes.Direct)
                player.attack += weapon.Atk;
            else if (weapon.AtkCalculation == Item.CalculationTypes.Percentage)
                player.attack += character.Atk * weapon.Atk / 100;
        }
    }

    private void CalculateDEF() {
        //damage = att * att / (att + def)
        player.defence = character.Def;
        if (headgear != null) {
            if (headgear.DefCalculation == Item.CalculationTypes.Direct)
                player.defence += headgear.Def;
            else if (headgear.DefCalculation == Item.CalculationTypes.Percentage)
                player.defence += character.Def * headgear.Def / 100;
        }
        if (apparel != null) {
            if (apparel.DefCalculation == Item.CalculationTypes.Direct)
                player.defence += apparel.Def;
            else if (apparel.DefCalculation == Item.CalculationTypes.Percentage)
                player.defence += character.Def * apparel.Def / 100;
        }

        if (player.defence < 1)
            player.defence = 1;
    }

    private void CalculateSPD() {
        player.speed = character.Spd;
        if (headgear != null) {
            if (headgear.SpdCalculation == Item.CalculationTypes.Direct)
                player.speed += headgear.Spd;
            else if (headgear.SpdCalculation == Item.CalculationTypes.Percentage)
                player.speed += character.Spd * headgear.Spd / 100;
        }
        if (apparel != null) {
            if (apparel.SpdCalculation == Item.CalculationTypes.Direct)
                player.speed += apparel.Spd;
            else if (apparel.SpdCalculation == Item.CalculationTypes.Percentage)
                player.speed += character.Spd * apparel.Spd / 100;
        }

        if (player.speed < 1)
            player.speed = 1;
    }

    private void SetupKnight(AnimatorOverrideController anim) {
        anim["Template_Sword_0-0"] = weapon.animations[0];
        anim["Template_Sword_0-1"] = weapon.animations[1];
        anim["Template_Sword_0-2"] = weapon.animations[2];
        anim["Template_Sword_0-3"] = weapon.animations[3];
        if (utility != null) {
            anim["Knight_Block-0"] = Knight_Block_1;
            anim["Template_Shield_0-0"] = utility.animations[0];
            anim["Template_Shield_0-1"] = utility.animations[1];
            anim["Template_Shield_0-2"] = utility.animations[2];
        } else {
            player.transform.Find("Arm_1").GetChild(0).gameObject.SetActive(false);
        }
        //player.gameObject.AddComponent<Knight>().SwordBeam = SwordBeam;
    }

    private void SetupArcher(AnimatorOverrideController anim) {
        anim["Template_Sword_0-0"] = weapon.animations[0];
        anim["Template_Sword_0-1"] = weapon.animations[1];
        anim["Template_Sword_0-2"] = weapon.animations[2];
        anim["Template_Sword_0-3"] = weapon.animations[3];
        player.gameObject.AddComponent<Archer>();
    }
}
