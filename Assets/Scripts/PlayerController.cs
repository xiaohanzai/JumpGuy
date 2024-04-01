using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 1f;

    private bool isGrounded;

    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 originalPos;

    public Transform bottomBoundMarker;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalPos = transform.position;
    }

    void Update()
    {
        HandleJumping();
        HandleAnimation();
        HandleFalling();
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void HandleAnimation()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
        
        if (isGrounded)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isGrounded", true);
        }
        else
        {
            if (!animator.GetBool("isRunning"))
            {
                animator.SetBool("isRunning", true);
            }
            animator.SetTrigger("Jump");
        }
    }

    private void HandleFalling()
    {
        if (transform.position.y < bottomBoundMarker.position.y)
        {
            GameManager.instance.SetGameOver();
        }
    }

    public void Reset()
    {
        transform.position = originalPos;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.speed = 1f;
        animator.SetBool("isGrounded", true);
        animator.SetBool("isRunning", false);
        isGrounded = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            GameManager.instance.SetGameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            UIManager.instance.SetScore(1);
            other.gameObject.SetActive(false);
        }
    }

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.speed = 0f;
    }
}
