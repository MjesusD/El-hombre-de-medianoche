using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configuraci�n de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Controles")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;

    private float horizontalInput;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    //variable para detectar objetos interactuables cercanos
    private InteractionObject currentInteractable;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        //obtener input horizontal (A/D o Flechas Izquierda/Derecha)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //determinar si esta en movimiento
        isMoving = horizontalInput != 0;

        //voltear sprite segun direccion
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; //izquierda
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; //derecha
        }

        //actualizar animacion
        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }
    }

    void MovePlayer()
    {
        //mover al jugador horizontalmente
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    void HandleInteractionInput()
    {
        //presionar E para interactuar con objetos cercanos
        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
    }
    //detectar cuando entramos en rango de un objeto interactuable
    void OnTriggerEnter2D(Collider2D other)
    {
        InteractionObject interactable = other.GetComponent<InteractionObject>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            interactable.ShowPrompt(true);
        }
    }

    //detectar cuando salimos del rango
    void OnTriggerExit2D(Collider2D other)
    {
        InteractionObject interactable = other.GetComponent<InteractionObject>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            interactable.ShowPrompt(false);
        }
    }

    //metodo publico para bloquear movimiento
    public void SetCanMove(bool canMove)
    {
        enabled = canMove;
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null)
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
