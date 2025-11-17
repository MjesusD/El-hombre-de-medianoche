using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Configuraci�n de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Controles")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    //[SerializeField] private KeyCode inventoryKey = KeyCode.I;

    private bool canMove = true;
    private float horizontalInput;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    // Variable para detectar objetos interactuables cercanos
    private InteractionObject currentInteractable;

    void Awake()
    {
        Instance = this;  
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Debug.Log($"[Player] Rigidbody asignado: {rb != null}, Scene: {gameObject.scene.name}");


    }

    void Update()
    {
        HandleMovementInput();
        HandleInteractionInput();
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleMovementInput()
    {
        if (!canMove)
        {
            horizontalInput = 0;
            isMoving = false;
            if (animator != null)
                animator.SetBool("isWalking", false);
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        isMoving = horizontalInput != 0;

        if (horizontalInput < 0)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0)
            spriteRenderer.flipX = false;

        if (animator != null)
            animator.SetBool("isWalking", isMoving);
    }


    void MovePlayer()
    {
        // Mover al jugador horizontalmente
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    void HandleInteractionInput()
    {
        // Presionar E para interactuar con objetos cercanos
        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
    }
    // Detectar cuando entramos en rango de un objeto interactuable
    void OnTriggerEnter2D(Collider2D other)
    {
        InteractionObject interactable = other.GetComponent<InteractionObject>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            interactable.ShowPrompt(true);
        }
    }

    // Detectar cuando salimos del rango
    void OnTriggerExit2D(Collider2D other)
    {
        InteractionObject interactable = other.GetComponent<InteractionObject>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            interactable.ShowPrompt(false);
        }
    }

    // Metodo publico para bloquear movimiento
    public void SetCanMove(bool canMove)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null)
                animator.SetBool("isWalking", false);
        }

        // Solo desactiva el movimiento, no todo el script
        this.enabled = true; // Aseguramos que nunca quede deshabilitado completamente
        moveSpeed = canMove ? 5f : 0f; // Velocidad 0 si está bloqueado
    }



    public bool IsMoving()
    {
        return isMoving;
    }
}
