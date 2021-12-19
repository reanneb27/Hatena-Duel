using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public enum CharacterState
    {
        IDLE, RUNNING, JUMPING, FALLING, DEATH
    }

    public ArenaAudioManagerScript AudioManager;

    // components
    public Animator animator;
    public Rigidbody2D RB2D;
    public SpriteRenderer SR;

    public void ChangeAnimationState(CharacterState state)
    {
        string cleanCharName = characterName.ToLower().Replace("'", "").Replace(" ", "");
        switch (state)
        {
            case CharacterState.IDLE:
                animator.Play(cleanCharName + "_" + "idle");
                break;
            case CharacterState.RUNNING:
                animator.Play(cleanCharName + "_" + "run");
                break;
            case CharacterState.JUMPING:
                animator.Play(cleanCharName + "_" + "jump");
                break;
            case CharacterState.FALLING:
                animator.Play(cleanCharName + "_" + "fall");
                break;
            case CharacterState.DEATH:
                isDead = true;
                StartCoroutine(PlayDeathAnimationWhenGrounded(cleanCharName));
                break;
            default:
                break;
        }
    }

    IEnumerator PlayDeathAnimationWhenGrounded(string cleanCharName)
    {
        while (!isGrounded)
            yield return new WaitForSeconds(0.1f);

        animator.Play(cleanCharName + "_" + "death");
        yield return null;
    }

    // character keyboard settings
    public SettingsSerializer PlayerSettings;

    // character attribs
    public string characterName;
    public float moveSpeed;

    public float jumpForce;
    public int currentJumpCount;
    public int maxJumpCount;

    public bool IsAI;
    public bool isDead;
    public bool isGrounded;

    public float maxHealth;
    public float health;
    public float maxRage = 100;
    public float rage;


    // skill info
    public List<GameObject> SkillList;
    public float Skill1Cooldown;
    public float Skill2Cooldown;
    public float Skill3Cooldown;
    public float UltimateSkillDuration;

    public float NextSkill1Time;
    public float NextSkill2Time;
    public float NextSkill3Time;
    public float UltimateFinishTime;

    public bool Skill1IsOnCD = false;
    public bool Skill2IsOnCD = false;
    public bool Skill3IsOnCD = false;
    public bool UltimateSkillIsRunning = false;

    public string Skill1Name;
    public string Skill2Name;
    public string Skill3Name;
    public string UltimateSkillName;

    protected virtual void Movement()
    {
        // left and right
        if (Input.GetKey(PlayerSettings.LeftKey))
            MoveLeft();
        else if (Input.GetKey(PlayerSettings.RightKey))
            MoveRight();
        else
            if (isGrounded)
                ChangeAnimationState(CharacterState.IDLE);

        // jump
        if (Input.GetKeyDown(PlayerSettings.JumpKey) && currentJumpCount < maxJumpCount)
            DoJump();

        // falling
        if (RB2D.velocity.y < 0 && !isGrounded)
            ChangeAnimationState(CharacterState.FALLING);
    }
    protected virtual void Attack()
    {
        if (Input.GetKey(PlayerSettings.Skill1Key) && Time.time > NextSkill1Time)
            DoSkill1();

        if (Input.GetKey(PlayerSettings.Skill2Key) && Time.time > NextSkill2Time)
            DoSkill2();

        if (Input.GetKey(PlayerSettings.Skill3Key) && Time.time > NextSkill3Time)
            DoSkill3();

        if (Input.GetKey(PlayerSettings.UltimateAttKey) && !UltimateSkillIsRunning && rage >= maxRage) // can ult
            DoUltimate();
        if (UltimateSkillIsRunning)
            DoingUltimate();
    }
    protected virtual void AIUpdate() { }
    protected abstract void DoSkill1();
    protected abstract void DoSkill2();
    protected abstract void DoSkill3();
    protected abstract void DoUltimate();
    protected abstract void DoingUltimate();
    protected virtual void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        SR.flipX = true;

        if (isGrounded)
            ChangeAnimationState(CharacterState.RUNNING);
    }

    protected virtual void MoveRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        SR.flipX = false;

        if (isGrounded)
            ChangeAnimationState(CharacterState.RUNNING);
    }
    protected virtual void DoJump()
    {
        RB2D.velocity = new Vector2();
        RB2D.angularVelocity = 0;
        RB2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
        currentJumpCount += 1;
        ChangeAnimationState(CharacterState.JUMPING);
    }
    public abstract void OnHit(float dmg, float _rage);

    void Update()
    {
        if (!isDead)
        {
            if (!IsAI)
            {
                Movement();
                Attack();
            }
            else
                AIUpdate();
        }
    }
}
