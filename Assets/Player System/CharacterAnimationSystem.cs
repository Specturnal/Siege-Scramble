using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationSystem : MonoBehaviour
{
    private Entity entity;

    public Character character;

    private SpriteRenderer leg_0, leg_1;
    private AnimatorOverrideController anim;

    public AnimationClip TemplateLeg_0;

    private void Start() {
        entity = GetComponent<Entity>();
        anim = (AnimatorOverrideController)entity.anim.runtimeAnimatorController;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            anim["Template_Shield_0-0"] = TemplateLeg_0;
        }
    }

    public void UpdateLeg_0(int state) {
        /*if (character.leg_0Clip[state] == null) {
            leg_0.sprite = character.leg_0Sprite[state];
        } else {
            anim["Template_Shield_0-0"] = character.leg_0Clip[0];
        }*/
    }
}