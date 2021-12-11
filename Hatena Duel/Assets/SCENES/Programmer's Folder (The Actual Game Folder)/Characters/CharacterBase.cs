using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    // character keyboard settings
    public SettingsSerializer PlayerSettings;

    // character attribs
    public string characterName;
    public float moveSpeed;

    public float jumpForce;
    public int currentJumpCount;
    public int maxJumpCount;

    public bool isGrounded;

    public float maxHealth;
    public float health;
    public float maxRage = 100;
    public float rage;


    // skill info
    public float Skill1Cooldown;
    public float Skill2Cooldown;
    public float Skill3Cooldown;
    public float UltimateSkillDuration;

    public float NextSkill1Time;
    public float NextSkill2Time;
    public float NextSkill3Time;
    public float NextUltimateTime;

    public bool Skill1IsOnCD = false;
    public bool Skill2IsOnCD = false;
    public bool Skill3IsOnCD = false;
    public bool UltimateSkillIsRunning = false;

    public string Skill1Name;
    public string Skill2Name;
    public string Skill3Name;
    public string UltimateSkillName;

    protected abstract void Movement();
    protected abstract void Attack();

    void Update()
    {
        Movement();
        Attack();
    }
}
