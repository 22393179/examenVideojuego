using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Propiedades Físicas Públicas")]
    public float movementSpeed = 5f;
    public float jumpForce = 8f;    
    public float playerMass = 1f;  
    public float gravityScale = 3f; 

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector3 startPosition;
    private bool isGrounded = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = playerMass;
        rb.gravityScale = gravityScale; 
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * movementSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Método llamado por la acción 'Jump' del Input System
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            // Aplicar la fuerza de salto
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone")) 
        {
            transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
}