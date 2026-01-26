using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    public PlayerStates currentState;
    public float invisDuration;
    private Color spriteColor;
    [SerializeField] private float moveSpeed = 5;
    public Vector2 moveInput;
    public Vector2 lastMoveInput;

    private void Start()
    {
        currentState = PlayerStates.Idle;
        spriteColor = playerSprite.color;
    }
    private void Update()
    {
        switch (currentState)
        {
            case PlayerStates.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerStates.Walking:
                rb.linearVelocity = moveSpeed * moveInput;
                break;
        }
    }
    public void Invis(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(ActivateInvis());
        }
    }

    private IEnumerator ActivateInvis()
    {
        Color tmpColor = spriteColor;
        tmpColor.a = 0.5f;
        playerSprite.color = tmpColor;
        yield return new WaitForSeconds(invisDuration);
        tmpColor.a = 1f;
        playerSprite.color = tmpColor;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
            lastMoveInput = moveInput;
            ChangeState(PlayerStates.Walking);
        }
        else if (context.canceled) { 
            animator.SetFloat("LastInputX", lastMoveInput.x);
            animator.SetFloat("LastInputY", lastMoveInput.y);
            ChangeState(PlayerStates.Idle);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            ChangeState(PlayerStates.Attacking);
            Debug.Log("Attacking");
        }
    }

    public void ChangeState(PlayerStates newState) {
        if (currentState == PlayerStates.Idle) animator.SetBool("isIdle", false);
        if (currentState == PlayerStates.Walking) animator.SetBool("isWalking", false);
        if (currentState == PlayerStates.Walking) animator.SetBool("isAttacking", false);

        currentState= newState;

        if (currentState == PlayerStates.Idle) animator.SetBool("isIdle", true);
        if (currentState == PlayerStates.Walking) animator.SetBool("isWalking", true);
        if (currentState == PlayerStates.Walking) animator.SetBool("isAttacking", true);
    }
}
public enum PlayerStates { Idle, Walking, Attacking, Invisible};