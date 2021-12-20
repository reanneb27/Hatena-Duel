using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArkaynScript : CharacterBase
{
    // players info
    CharacterBase enemy;
    string enemyLayer = "Player1"; // player tag is the same as its layer
    GameObject enemyGameObj;

    // Start is called before the first frame update
    void Awake()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        animator = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        SkillList = new List<GameObject>() { HellsFlames, HeavensGate, AcidRain };

        // init character attribs
        characterName = "Arkayn";

        moveSpeed = 8f;
        jumpForce = 5f;

        currentJumpCount = 0;
        maxJumpCount = 3;
        isGrounded = true;

        Skill1Cooldown = 3;
        Skill2Cooldown = 9;
        Skill3Cooldown = 5;
        UltimateSkillDuration = 5;

        Skill1Name = Skill1;
        Skill2Name = Skill2;
        Skill3Name = Skill3;
        UltimateSkillName = UltimateSkill;

        maxHealth = 1300;
        health = 1300;
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
                    actionFinishTime = Time.time + Random.Range(1, 7);
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
                    actionFinishTime = Time.time + Random.Range(1, 3);
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
            DoSkill1();
        if (Time.time > NextSkill2Time)
            if (Random.Range(1, 1000) <= 5)
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
    public static string Skill1 = "Hell's Flames";
    public static string Skill2 = "Heaven's Gate";
    public static string Skill3 = "Acid Rain";
    public static string UltimateSkill = "Divine Intervention";
    public static string Skill1Desc = "Ignites the ground, dealing damage over time.";
    public static string Skill2Desc = "Creates a wall that blocks the path.";
    public static string Skill3Desc = "Rains acid. Enemy be damaged while Arkayn will heal under the rain.";
    public static string UltimateSkillDesc = "Heals until rage is depleted. If Arkayn dies during ultimate, he revives with half health and 0 rage.";
}