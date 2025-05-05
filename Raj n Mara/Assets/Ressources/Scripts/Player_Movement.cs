using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpForce = 10f;

    [Header("Dashing")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Slashing")]
    public float slashDamage = 15f;
    public float slashDuration = 0.2f;
    public float slashCooldown = 0.5f;
    public BoxCollider2D hitBoxSlash;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;

    private Animator anim;
    private float dashTime;
    private float dashCooldownTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        hitBoxSlash.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isDashing)
        {
            // Handle horizontal movement
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

            // Animation: Move or Idle
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                anim.SetTrigger("Move");
                transform.localScale = new Vector3(moveInput > 0 ? 1 : -1, 1, 1); // Flip character
            }

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("Jump");
            }
        }

        // Handle dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            anim.SetTrigger("Dash");
            StartDash();
        }

        // Cooldown timer
        if (!canDash && Time.time >= dashCooldownTime)
        {
            canDash = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //SlashAttack();
        }
    }

    void DeactivateHitbox()
    {
        hitBoxSlash.gameObject.SetActive(false);
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTime = Time.time + dashDuration;
        dashCooldownTime = Time.time + dashCooldown;

        float dashDirection = transform.localScale.x > 0 ? 1 : -1; // Dash in the facing direction
        rb.linearVelocity = new Vector2(dashDirection * dashForce, 0); // Ignore vertical velocity for clean dash
        anim.SetTrigger("Dash");
    }

    void FixedUpdate()
    {
        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }
    }

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal dash velocity
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the ground check in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
