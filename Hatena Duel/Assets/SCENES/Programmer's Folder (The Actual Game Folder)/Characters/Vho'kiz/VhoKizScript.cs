using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VhoKizScript : MonoBehaviour
{
    private float moveSpeed = 17f;
    private float jumpForce = 25f;


    private int currentJumpCount = 0;
    private int maxJumpCount = 2;
    private bool isGrounded = true;

    private Rigidbody2D RB2D;
    private Animator animator;
    private SpriteRenderer SR;

    private enum VhoKizState
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
                ChangeAnimationState(VhoKizState.RUNNING);
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
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
            Debug.Log(collision.gameObject.name);
        }
    }
}
