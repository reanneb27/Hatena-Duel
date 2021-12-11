using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VhoKizScript : CharacterBase
{
    private Rigidbody2D RB2D;
    private Animator animator;
    private SpriteRenderer SR;

    private enum VhoKizState
    {
        IDLE, RUNNING, JUMPING, FALLING
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        // init character attribs
        characterName = "Vho'Kiz";

        moveSpeed = 17f;
        jumpForce = 25f;

        currentJumpCount = 0;
        maxJumpCount = 2;
        isGrounded = true;

        Skill1Cooldown = 2;
        Skill2Cooldown = 3;
        Skill3Cooldown = 5;
        UltimateSkillDuration = 5;

        Skill1Name = Skill1;
        Skill2Name = Skill2;
        Skill3Name = Skill3;
        UltimateSkillName = UltimateSkill;

        maxHealth = 1000;
        health = 1000;
        maxRage = 100;
        rage = 0;
}

    protected override void Movement()
    {
        // left and right
        if (Input.GetKey(PlayerSettings.LeftKey))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            SR.flipX = true;

            if (isGrounded)
                ChangeAnimationState(VhoKizState.RUNNING);
        }
        else if (Input.GetKey(PlayerSettings.RightKey))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            SR.flipX = false;

            if (isGrounded)
                ChangeAnimationState(VhoKizState.RUNNING);
        }
        else
        {
            if (isGrounded)
                ChangeAnimationState(VhoKizState.IDLE);
        }

        // jump
        if (Input.GetKeyDown(PlayerSettings.JumpKey) && currentJumpCount < maxJumpCount)
        {
            RB2D.velocity = new Vector2();
            RB2D.angularVelocity = 0;
            RB2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            currentJumpCount += 1;
            ChangeAnimationState(VhoKizState.JUMPING);
        }

        // falling
        if (RB2D.velocity.y < 0 && !isGrounded)
        {
            ChangeAnimationState(VhoKizState.FALLING);
        }
    }

    private void ChangeAnimationState(VhoKizState state)
    {
        switch (state)
        {
            case VhoKizState.IDLE:
                animator.Play("vhokiz_idle");
                break;
            case VhoKizState.RUNNING:
                animator.Play("vhokiz_run");
                break;
            case VhoKizState.JUMPING:
                animator.Play("vhokiz_jump");
                break;
            case VhoKizState.FALLING:
                animator.Play("vhokiz_fall");
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // grounded
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            currentJumpCount = 0;
            ChangeAnimationState(VhoKizState.IDLE);
            Debug.Log("collided!");
        }
        else
        {
            if (collision.gameObject.tag != "Wall")
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
                Debug.Log("IGNORED BECAUSE NOT GROUND/WALL");
            }
            else
                Debug.Log(collision.gameObject.name);
        }
    }

    

    // skill info
    public static string Skill1 = "Conscious Missiles";
    public static string Skill2 = "Magnetic Star";
    public static string Skill3 = "Supernova";
    public static string UltimateSkill = "Control Matter";
    public static string Skill1Desc = "Decreases CD by 80% if enemy is hit.";
    public static string Skill2Desc = "Follows the opponent faster if far, slower if near, for 4 seconds.";
    public static string Skill3Desc = "Fires a laser at the direction of the Vhokiz to the cursor.";
    public static string UltimateSkillDesc = "Skills become bigger; Vhokiz movements are enhanced.";
}
