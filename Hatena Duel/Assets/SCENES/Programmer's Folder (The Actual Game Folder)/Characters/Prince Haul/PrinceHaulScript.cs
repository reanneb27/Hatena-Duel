using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceHaulScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;


    private int currentJumpCount = 0;
    private int maxJumpCount = 2;
    private bool isGrounded = true;

    private Rigidbody2D RB2D;
    private Animator animator;
    private SpriteRenderer SR;

    private enum PrinceHaulState
    {
        IDLE, RUNNING, JUMPING, FALLING
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        RB2D = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // left and right
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            SR.flipX = true;

            if (isGrounded)
                ChangeAnimationState(PrinceHaulState.RUNNING);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            SR.flipX = false;

            if (isGrounded)
                ChangeAnimationState(PrinceHaulState.RUNNING);
        }
        else
        {
            if (isGrounded)
                ChangeAnimationState(PrinceHaulState.IDLE);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            RB2D.velocity = new Vector2();
            RB2D.angularVelocity = 0;
            RB2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            currentJumpCount += 1;
            ChangeAnimationState(PrinceHaulState.JUMPING);
        }

        // falling
        if (RB2D.velocity.y < 0 && !isGrounded)
        {
            ChangeAnimationState(PrinceHaulState.FALLING);
        }
    }

    private void ChangeAnimationState(PrinceHaulState state)
    {
        switch (state)
        {
            case PrinceHaulState.IDLE:
                animator.Play("princeHaul_idle");
                break;
            case PrinceHaulState.RUNNING:
                animator.Play("princeHaul_run");
                break;
            case PrinceHaulState.JUMPING:
                animator.Play("princeHaul_jump");
                break;
            case PrinceHaulState.FALLING:
                animator.Play("princeHaul_fall");
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
            ChangeAnimationState(PrinceHaulState.IDLE);
        }
    }

}