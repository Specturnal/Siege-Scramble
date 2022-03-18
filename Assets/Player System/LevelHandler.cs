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
        player = SpawnCharacter(Vector2.zero).GetComponent<Player>();
    }

    public Entity SpawnCharacter(Vector2 position) {
        Entity entity = Instantiate(playerEntity, position, Quaternion.identity).GetComponent<Entity>();
        //player.character = character;
        //player.GetComponent<CharacterAnimationSystem>().character = character;
        CalculateHP(entity);
        CalculateATK(entity);
        CalculateDEF(entity);
        CalculateSPD(entity);

        AnimatorOverrideController anim = Instantiate(character.animator);

        if (headgear != null) {
            Transform playerHead = entity.transform.Find("Head");
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
                SetupKnight(entity, anim); break;
            case Item.HeroClassTypes.Archer:
                SetupArcher(entity, anim); break;
        }

        entity.anim.runtimeAnimatorController = anim;

        return entity;
    }

    private void CalculateHP(Entity entity) {
        entity.health = character.Hp;
        if (headgear != null) {
            if (headgear.HpCalculation == Item.CalculationTypes.Direct)
                entity.health += headgear.Hp;
            else if (headgear.HpCalculation == Item.CalculationTypes.Percentage)
                entity.health += character.Hp * headgear.Hp / 100;
        }
        if (apparel != null) {
            if (apparel.HpCalculation == Item.CalculationTypes.Direct)
                entity.health += apparel.Hp;
            else if (apparel.HpCalculation == Item.CalculationTypes.Percentage)
                entity.health += character.Hp * apparel.Hp / 100;
        }

        if (entity.health < 1)
            entity.health = 1;
    }

    private void CalculateATK(Entity entity) {
        entity.attack = character.Atk;
        if (weapon != null) {
            if (weapon.AtkCalculation == Item.CalculationTypes.Direct)
                entity.attack += weapon.Atk;
            else if (weapon.AtkCalculation == Item.CalculationTypes.Percentage)
                entity.attack += character.Atk * weapon.Atk / 100;
        }
    }

    private void CalculateDEF(Entity entity) {
        //damage = att * att / (att + def)
        entity.defense = character.Def;
        if (headgear != null) {
            if (headgear.DefCalculation == Item.CalculationTypes.Direct)
                entity.defense += headgear.Def;
            else if (headgear.DefCalculation == Item.CalculationTypes.Percentage)
                entity.defense += character.Def * headgear.Def / 100;
        }
        if (apparel != null) {
            if (apparel.DefCalculation == Item.CalculationTypes.Direct)
                entity.defense += apparel.Def;
            else if (apparel.DefCalculation == Item.CalculationTypes.Percentage)
                entity.defense += character.Def * apparel.Def / 100;
        }

        if (entity.defense < 1)
            entity.defense = 1;
    }

    private void CalculateSPD(Entity entity) {
        entity.speed = character.Spd;
        if (headgear != null) {
            if (headgear.SpdCalculation == Item.CalculationTypes.Direct)
                entity.speed += headgear.Spd;
            else if (headgear.SpdCalculation == Item.CalculationTypes.Percentage)
                entity.speed += character.Spd * headgear.Spd / 100;
        }
        if (apparel != null) {
            if (apparel.SpdCalculation == Item.CalculationTypes.Direct)
                entity.speed += apparel.Spd;
            else if (apparel.SpdCalculation == Item.CalculationTypes.Percentage)
                entity.speed += character.Spd * apparel.Spd / 100;
        }

        if (entity.speed < 1)
            entity.speed = 1;
    }

    private void SetupKnight(Entity entity, AnimatorOverrideController anim) {
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
            entity.transform.Find("Arm_1").GetChild(0).gameObject.SetActive(false);
        }
        //player.gameObject.AddComponent<Knight>().SwordBeam = SwordBeam;
    }

    private void SetupArcher(Entity entity, AnimatorOverrideController anim) {
        anim["Template_Sword_0-0"] = weapon.animations[0];
        anim["Template_Sword_0-1"] = weapon.animations[1];
        anim["Template_Sword_0-2"] = weapon.animations[2];
        anim["Template_Sword_0-3"] = weapon.animations[3];
        entity.gameObject.AddComponent<Archer>();
    }
}
