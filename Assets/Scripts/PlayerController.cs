using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Propiedades Físicas Públicas")]
    public float movementSpeed = 5f; // Control de Velocidad
    public float jumpForce = 8f;     // Control de Fuerza (Salto)
    public float playerMass = 1f;    // Control de Masa
    public float gravityScale = 3f;  // Control de Gravedad

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector3 startPosition;
    private bool isGrounded = false; // Necesario para evitar saltos infinitos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Asignar propiedades públicas al Rigidbody2D al inicio
        rb.mass = playerMass;
        rb.gravityScale = gravityScale; 
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Aplicar la velocidad de movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput.x * movementSpeed, rb.linearVelocity.y);
    }

    // Método llamado por la acción 'Move' del Input System
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
            isGrounded = false; // Evita doble salto
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Esta línea verifica si la colisión fue con el objeto etiquetado como "DeathZone"
        if (other.CompareTag("DeathZone")) 
        {
            // El personaje deberá reaparecer en el punto de partida.
            transform.position = startPosition; // <-- 3. Vuelve a la posición guardada
            rb.linearVelocity = Vector2.zero;         // <-- Detiene todo movimiento para un reinicio limpio
        }
    }

    // Lógica para detectar si el personaje está en el suelo (Grounded)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Asume que todos los objetos en el layer "Ground" son plataformas
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
}