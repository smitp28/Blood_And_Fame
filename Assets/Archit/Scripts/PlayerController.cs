using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    public PlayerStates currentState;
    public float invisDuration;
    private Color spriteColor;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float nightSpeed = 10f;
    private float currentSpeed;
    public Vector2 moveInput;
    public Vector2 lastMoveInput;
    public LayerMask victims;
    public Transform attackPoint;
    public float attackRange;
    public float attackTime;
    public bool canMove;
    public AudioClip eatingBones;
    public GameObject killingScreen;
    public GameObject victim;
    private void Start()
    {
        currentState = PlayerStates.Idle;
        spriteColor = playerSprite.color;
        canMove = true;
        currentSpeed = moveSpeed;
    }
    private void Update()
    {
        switch (currentState)
        {
            case PlayerStates.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerStates.Walking:
                canMove = true;
                rb.linearVelocity = currentSpeed * moveInput;
                break;
            case PlayerStates.Attacking:
                rb.linearVelocity = Vector2.zero;
                canMove = false;
                break;
        }

        victim = GameObject.FindGameObjectWithTag("victims");
    }
    private void FixedUpdate()
{
    if (currentState == PlayerStates.Walking)
    {
        rb.linearVelocity = moveInput.normalized * currentSpeed;
    }
    else
    {
        rb.linearVelocity = Vector2.zero;
    }
    

}

    public void Invis(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(ActivateInvis());
        }
    }

    public void EnableNightSpeed()
    { 
         currentSpeed = nightSpeed;
         
    }

    public void DisableNightSpeed()
    {
       currentSpeed = moveSpeed;
       
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
        if (canMove == false)
        { return; }
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
            lastMoveInput = moveInput;
            ChangeState(PlayerStates.Walking);
        }
        else if (context.canceled) {
            moveInput = Vector2.zero;
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
            StartCoroutine(Kill());
            UnityEngine.Debug.Log("Attacking");
        }
    }

    public void ChangeState(PlayerStates newState) {
        if (currentState == PlayerStates.Idle) animator.SetBool("isIdle", false);
        if (currentState == PlayerStates.Walking) animator.SetBool("isWalking", false);
        if (currentState == PlayerStates.Attacking) animator.SetBool("isAttacking", false);

        currentState= newState;

        if (currentState == PlayerStates.Idle) animator.SetBool("isIdle", true);
        if (currentState == PlayerStates.Walking) animator.SetBool("isWalking", true);
        if (currentState == PlayerStates.Attacking) animator.SetBool("isAttacking", true);
    }

    private IEnumerator Kill()
    {
        Collider2D Victim = Physics2D.OverlapCircle(attackPoint.position, attackRange, victims);
        if (Victim != null && victim.GetComponent<Npc_Victims>().isDead==false)
        { 
            Victim.GetComponent<Npc_Victims>().isDead = true;
            InsanityMeter.instance.ApplyInsanity(-15f);
            Victim.GetComponent<Npc_Victims>().anim.SetBool("isDead", true);
            AudioManager.instance.PlaySoundFx(eatingBones, transform, 1f);
            killingScreen.SetActive(true);
            killingScreen.GetComponentInChildren<Animator>().Play("KillingAnimation");
            ChangeState(PlayerStates.Attacking);
            yield return new WaitForSeconds(attackTime);
            killingScreen.SetActive(false);
            ChangeState(PlayerStates.Walking);
        }
        
    }
}
public enum PlayerStates { Idle, Walking, Attacking, Invisible};