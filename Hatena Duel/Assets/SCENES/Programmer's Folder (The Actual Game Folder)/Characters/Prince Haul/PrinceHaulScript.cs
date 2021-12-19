using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PrinceHaulScript : CharacterBase
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        animator = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        SkillList = new List<GameObject>() { Boomerang, Endure, CopyCat};

        // init character attribs
        characterName = "Prince Haul";

        moveSpeed = 10f;
        jumpForce = 10f;

        currentJumpCount = 0;
        maxJumpCount = 2;
        isGrounded = true;

        Skill1Cooldown = 1;
        Skill2Cooldown = 8;
        Skill3Cooldown = 4;
        UltimateSkillDuration = 5;

        Skill1Name = Skill1;
        Skill2Name = Skill2;
        Skill3Name = Skill3;
        UltimateSkillName = UltimateSkill;

        maxHealth = 1700;
        health = 1700;
        maxRage = 100;
        rage = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // grounded
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            currentJumpCount = 0;
            ChangeAnimationState(CharacterState.IDLE);
        }
    }


    // skill info
    public static string Skill1 = "Boomerang";
    public static string Skill2 = "Endure";
    public static string Skill3 = "Copy Cat";
    public static string UltimateSkill = "Adrenaline Rush";
    public static string Skill1Desc = "Throws a boomerang, dealing damage on 1st hit and returning hit.";
    public static string Skill2Desc = "Decrease damage taken by 80% and increase rage taken on hit for some time.";
    public static string Skill3Desc = "Gets a random non-ultimate ability from the enemy but damage is decreased by 50%.";
    public static string UltimateSkillDesc = "Cooldown is greatly decreased.";
}