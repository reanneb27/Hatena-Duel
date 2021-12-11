using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkaynScript : MonoBehaviour
{
    private float moveSpeed = 8f;
    private float jumpForce = 5f;


    private int currentJumpCount = 0;
    private int maxJumpCount = 3;
    private bool isGrounded = true;

    private Rigidbody2D RB2D;
    private Animator animator;
    private SpriteRenderer SR;

    private enum ArkaynState
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
                ChangeAnimationState(ArkaynState.RUNNING);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            SR.flipX = false;

            if (isGrounded)
                ChangeAnimationState(ArkaynState.RUNNING);
        }
        else
        {
            if (isGrounded)
                ChangeAnimationState(ArkaynState.IDLE);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            RB2D.velocity = new Vector2();
            RB2D.angularVelocity = 0;
            RB2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            currentJumpCount += 1;
            ChangeAnimationState(ArkaynState.JUMPING);
        }

        // falling
        if (RB2D.velocity.y < 0 && !isGrounded)
        {
            ChangeAnimationState(ArkaynState.FALLING);
        }
    }

    private void ChangeAnimationState(ArkaynState state)
    {
        switch (state)
        {
            case ArkaynState.IDLE:
                animator.Play("arkayn_idle");
                break;
            case ArkaynState.RUNNING:
                animator.Play("arkayn_run");
                break;
            case ArkaynState.JUMPING:
                animator.Play("arkayn_jump");
                break;
            case ArkaynState.FALLING:
                animator.Play("arkayn_fall");
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
            ChangeAnimationState(ArkaynState.IDLE);
            Debug.Log("collided!");
        }
        else
        {
            Debug.Log(collision.gameObject.name);
        }
    }


}