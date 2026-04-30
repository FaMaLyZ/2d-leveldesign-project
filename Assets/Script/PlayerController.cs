using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public float knockback = 10f;

    [Header("Gravity")]
    public float gravityMultiplier = 1f;

    [Header("Jump Feel")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    [Header("Attack")]
    public float attackCooldown = 0.4f;
    public float attackDuration = 0.2f;
    public float attackRange = 1.2f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    [Header("Platform Drop")]
    public float dropCooldown = 1f;
    public LayerMask groundLayer;
    public Collider2D[] platformColliders;

    [Header("Visual")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStat playerStat;

    // --- Private state ---
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private InputAction jumpAction;
    private InputAction moveAction;
    private float horizontalInput;

    [SerializeField] private bool isOnGround = false;
    [SerializeField] private bool isDropping = false;

    // Coyote Time
    private float coyoteTimeCounter;

    // Jump Buffer
    private float jumpBufferCounter;

    // Attack
    private bool isAttacking = false;
    private float attackCooldownCounter = 0f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        // ใช้ gravityScale แทน Physics2D.gravity เพื่อไม่ affect objects อื่น
        rb.gravityScale = gravityMultiplier;

        playerStat = GetComponent<PlayerStat>();
        // หา platform colliders
        GameObject[] platformObjects = GameObject.FindGameObjectsWithTag("Platform");
        platformColliders = new Collider2D[platformObjects.Length];
        for (int i = 0; i < platformObjects.Length; i++)
        {
            platformColliders[i] = platformObjects[i].GetComponent<Collider2D>();
        }
        jumpAction = InputSystem.actions.FindAction("Jump");
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        HandleMovement();
        HandleCoyoteTime();
        HandleJumpBuffer();
        HandleJump();
        HandleDropThrough();
        HandleAttack();
        HandleAnimation();

        // ลด cooldown ของ attack ตามเวลา
        if (attackCooldownCounter > 0f)
            attackCooldownCounter -= Time.deltaTime;
    }

    // -------------------------------------------------------
    //  Movement
    // -------------------------------------------------------
    void HandleMovement()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        transform.Translate(horizontalInput * speed * Time.deltaTime * Vector2.right);
        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // -------------------------------------------------------
    //  TakeDamage knockback
    // -------------------------------------------------------
    
    public void DamageKnockback()
    {
        rb.AddForce(knockback * new Vector2(-1,1),ForceMode2D.Impulse);
    }

    // -------------------------------------------------------
    //  Coyote Time
    // -------------------------------------------------------
    void HandleCoyoteTime()
    {
        if (isOnGround)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    // -------------------------------------------------------
    //  Jump Buffer
    // -------------------------------------------------------
    void HandleJumpBuffer()
    {
        if (jumpAction.triggered)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    // -------------------------------------------------------
    //  Jump
    // -------------------------------------------------------
    void HandleJump()
    {
        // กระโดดได้ถ้า: มี jump buffer อยู่ และ coyote time ยังไม่หมด
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

            animator.SetTrigger("jump");
            // reset ทั้งคู่ทันที
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isOnGround = false;
        }
    }

    // -------------------------------------------------------
    //  Drop Through Platform (กด S)
    // -------------------------------------------------------
    void HandleDropThrough()
    {
        if (Input.GetKeyDown(KeyCode.S) && isOnGround && !isDropping)
        {
            // ตรวจว่ายืนอยู่บน Platform จริง ไม่ใช่ Ground ถาวร
            Collider2D hit = Physics2D.OverlapCircle(new Vector2 (transform.position.x,transform.position.y - 2), 0.15f, groundLayer);
            if (hit != null && hit.CompareTag("Platform"))
            {
                Debug.Log("s");
                StartCoroutine(DropThroughPlatform());
            }
        }
    }

    // -------------------------------------------------------
    //  Attack (กด Z)
    // -------------------------------------------------------
    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking && attackCooldownCounter <= 0f)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        Debug.Log("Attack");
        attackCooldownCounter = attackCooldown;

        // TODO: เรียก animator ที่นี่ได้เลย
        // animator.SetTrigger("Attack");
        animator.SetTrigger("attack");

        // Hitbox active ช่วงสั้น ๆ
        yield return new WaitForSeconds(0.05f); // delay เล็กน้อยให้ animation ขึ้นก่อน

        // ตรวจ overlap ศัตรู
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
            var enemtGo = enemy.gameObject.GetComponent<Enemy>();
            enemtGo.Attacked();
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    // -------------------------------------------------------
    //  Drop Through Platform (coroutine)
    // -------------------------------------------------------
    IEnumerator DropThroughPlatform()
    {
        isDropping = true;

        foreach (Collider2D platform in platformColliders)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(playerCollider, platform, true);
        }

        yield return new WaitForSeconds(dropCooldown);

        foreach (Collider2D platform in platformColliders)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(playerCollider, platform, false);
        }

        isDropping = false;
    }

    // -------------------------------------------------------
    //  Collision Detection
    // -------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isOnGround = false;
        }
    }

    // -------------------------------------------------------
    //  Gizmos (debug hitbox ใน Scene view)
    // -------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    ///Animetion 
    void HandleAnimation()
    {
        animator.SetFloat("speed", Mathf.Abs(horizontalInput));
        animator.SetBool("isGrounded", isOnGround);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }
}