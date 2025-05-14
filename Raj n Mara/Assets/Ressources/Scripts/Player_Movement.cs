using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public bool hasDoubleJumped = false;
    [Header("Dashing")]
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Slashing")]
    public float slashDuration = 0.5f;
    public float slashCooldown = 0.5f;
    public GameObject slasher; // Hitbox or slash effect
    public GameObject slashPrefabPreview;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;
    private bool canSlash = true;
    private float dashTime;
    private float dashCooldownTime;
    private float slashCooldownTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isDashing)
        {
            HandleMovement();
            HandleJumping();
        }

        HandleDashing();
        HandleSlashing();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

        // Animation: Move or Idle
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            anim.SetTrigger("Move");
            transform.localScale = new Vector3(moveInput > 0 ? 1 : -1, 1, 1); // Flip character
        }
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasDoubleJumped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
            hasDoubleJumped = true;
        }
        if (isGrounded) hasDoubleJumped = false;
    }

    void HandleDashing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }

        if (!canDash && Time.time >= dashCooldownTime)
        {
            canDash = true;
        }

        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }
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

    void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal dash velocity
    }

    void HandleSlashing()
    {
        if (Input.GetMouseButtonDown(0) && canSlash)
        {
            StartSlash();
        }

        if (!canSlash && Time.time >= slashCooldownTime)
        {
            canSlash = true;
        }
    }

    void StartSlash()
    {
        canSlash = false;
        slashCooldownTime = Time.time + slashCooldown;

        // Activate the slasher hitbox or effect
        if (slasher != null)
        {
            slasher.SetActive(true);
            Invoke(nameof(EndSlash), slashDuration); // Deactivate after duration
        }

        // Instantiate a visual effect (if needed)
        if (slashPrefabPreview != null)
        {
            GameObject slash = Instantiate(slashPrefabPreview, transform);
            Destroy(slash, slashDuration); // Destroy after slash duration
        }

        anim.SetTrigger("Slash");
    }

    void EndSlash()
    {
        if (slasher != null)
        {
            slasher.SetActive(false);
        }
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
