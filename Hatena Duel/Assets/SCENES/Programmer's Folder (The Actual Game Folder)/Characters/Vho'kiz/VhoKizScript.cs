using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VhoKizScript : CharacterBase
{
    // players info
    CharacterBase enemy;
    string enemyLayer = "Player1"; // player tag is the same as its layer
    GameObject enemyGameObj;

    void Awake()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        animator = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        SkillList = new List<GameObject>() { Rocket, Orb, Laser };

        // init character attribs
        characterName = "Vho'Kiz";

        moveSpeed = 17f;
        jumpForce = 25f;

        currentJumpCount = 0;
        maxJumpCount = 2;
        isGrounded = true;

        Skill1Cooldown = 2;
        Skill2Cooldown = 3;
        Skill3Cooldown = 7;
        UltimateSkillDuration = 9;

        Skill1Name = Skill1;
        Skill2Name = Skill2;
        Skill3Name = Skill3;
        UltimateSkillName = UltimateSkill;

        maxHealth = 1100;
        health = 1100;
        maxRage = 100;
        rage = 0;
    }

    void Start()
    {
        if (gameObject.tag == "Player1")
            enemyLayer = "Player2";
        enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);
        enemy = enemyGameObj.GetComponent<CharacterBase>();

    }


    bool moveToLeft;
    float actionFinishTime;
    int branchCount = 0;
    bool toggled = true;
    protected override void AIUpdate()
    {
        Vector2 yourPos = transform.position + new Vector3(0, SR.size.y / 2);
        Vector2 enemyPos = enemyGameObj.transform.position + new Vector3(0, enemy.SR.size.y / 2);

        if (toggled) // paused
        {
            if (Time.time > actionFinishTime)
            {
                if (branchCount == 1)
                {
                    toggled = !toggled;
                    branchCount = 0;
                }
                else
                {
                    actionFinishTime = Time.time + Random.Range(1, 3);
                    branchCount++;
                }
            }
            else if (Time.time <= actionFinishTime)
            {

            }
        }
        else // moving
        {
            if (Time.time > actionFinishTime)
            {
                if (isGrounded)
                    ChangeAnimationState(CharacterState.IDLE);
                if (branchCount == 1)
                {
                    toggled = !toggled;
                    branchCount = 0;
                }
                else
                {
                    moveToLeft = Random.Range(1, 10) < 5;
                    actionFinishTime = Time.time + Random.Range(1, 2);
                    branchCount++;
                }
            }
            else if (Time.time <= actionFinishTime)
            {
                if (moveToLeft)
                    MoveLeft();
                else 
                    MoveRight();
            }
            else
                if (isGrounded)
                    ChangeAnimationState(CharacterState.IDLE);
        }

        // random 3/1000% chance jump
        if (Random.Range(1, 1000) <= 3 && currentJumpCount < maxJumpCount)
            DoJump();

        // falling
        if (RB2D.velocity.y < 0 && !isGrounded)
            ChangeAnimationState(CharacterState.FALLING);

        SR.flipX = enemyPos.x < yourPos.x;
        // attacking
        if (Time.time > NextSkill1Time)
        {
            if (Mathf.Max(enemyPos.y, yourPos.y) - Mathf.Min(enemyPos.y, yourPos.y) < 10)
                DoSkill1();
        }
        if (Time.time > NextSkill2Time)
            DoSkill2();
        if (Time.time > NextSkill3Time)
            DoSkill3();
        if (!UltimateSkillIsRunning && rage >= maxRage)
            DoUltimate();
        if (UltimateSkillIsRunning)
            DoingUltimate();
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
    public static string Skill1 = "Conscious Missiles";
    public static string Skill2 = "Magnetic Star";
    public static string Skill3 = "Supernova";
    public static string UltimateSkill = "Control Matter";
    public static string Skill1Desc = "Decreases CD by 80% if enemy is hit.";
    public static string Skill2Desc = "Follows the opponent faster if far, slower if near, for 2.5 seconds.";
    public static string Skill3Desc = "Fires a laser at the direction of Vhokiz and his opponent.";
    public static string UltimateSkillDesc = "Skills become bigger; Vhokiz movements are enhanced.";
}
