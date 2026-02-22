using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem; // new Input System namespace

public class Player : MonoBehaviour
{
    public int health = 1;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int extraJumpValue = 1;
    private int extraJumps;

    // Input Actions
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInput = GetComponent<PlayerInput>();

        // Hook into Unity’s default preset
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Start()
    {
        extraJumps = extraJumpValue;
    }

    void Update()
{
    // Movement
    Vector2 moveInput = moveAction.ReadValue<Vector2>();
    rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

    if (isGrounded)
    {
        extraJumps = extraJumpValue;
    }

    // Jump
    if (jumpAction.triggered)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (extraJumps > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            extraJumps--;
        }
    }

    SetAnimation(moveInput.x);

    if(transform.position.y < -20f)
        {
            Die();
        }
}


    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
                animator.Play("Player_Idle");
            else
                animator.Play("Player_Run");
        }
        else
        {
            if (rb.linearVelocity.y > 0)
                animator.Play("Player_Jump");
            else
                animator.Play("Player_Fall");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            health -= 25;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StartCoroutine(BlinkRed());

            if (health <= 0)
                Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
