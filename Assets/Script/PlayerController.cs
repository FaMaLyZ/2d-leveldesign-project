using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;

    private Rigidbody2D rb;
    private InputAction jumpAction;
    private float horizontalInput;
    private InputAction moveAction;

    public LayerMask groundLayer;

    [SerializeField] private bool isDropping = false;
    [SerializeField] private bool isOnGround = true;
    public float dropCooldown = 1f;

    public Collider2D[] platformColliders;
    private Collider2D playerCollider;

    void Start()
    {
        Physics2D.gravity *= gravityMultiplier;
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
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
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        transform.Translate(horizontalInput * speed * Time.deltaTime * Vector2.right);

        if (jumpAction.triggered && isOnGround)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            isOnGround = false;
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("GOOT S uu");
            if (isOnGround && !isDropping)
            {
                Debug.Log(isOnGround);
                Debug.Log(isDropping);
                StartCoroutine(DropThroughPlatform());
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isOnGround = true;
            
        }
    }
    IEnumerator DropThroughPlatform()
    {
        isDropping = true;
        // ปิด collision กับ platform ชั่วคราว
        foreach (Collider2D platform in platformColliders)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(playerCollider, platform, true);
        }

        yield return new WaitForSeconds(dropCooldown);

        // เปิด collision กลับ
        foreach (Collider2D platform in platformColliders)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
        isDropping = false;
    }
}
