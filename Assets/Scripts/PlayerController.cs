using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Animator animator;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Vector2 lastMovement = Vector2.down;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);

        bool isMoving = movement.sqrMagnitude > 0.001f;

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);

            if (isMoving)
            {
                lastMovement = movement.normalized;
            }

            animator.SetFloat("LastInputX", lastMovement.x);
            animator.SetFloat("LastInputY", lastMovement.y);
        }
    }
}