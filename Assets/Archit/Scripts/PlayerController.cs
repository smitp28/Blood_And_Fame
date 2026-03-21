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
    [SerializeField] private float daySpeed = 5f;
    [SerializeField] private float cleanSpeed;
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
    public Transform playerFacingTowards;
    public GameObject killingScreen;
    private GameObject victim;
    private bool isKilling = false;
    public GameObject Corpseprefab;
    public GameObject corpseInstance;
    public bool isCleaning = false;
    private bool isDay = true;
    public static  PlayerController instance;

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); return; }
    }
    private void Start()
    {
        lastMoveInput = Vector2.up;
        currentState = PlayerStates.Idle;
        spriteColor = playerSprite.color;
        canMove = true;
        currentSpeed = daySpeed;
    }
    private void Update()
    {
        victim = GameObject.FindGameObjectWithTag("victims");
        AimRotation();
    }
    private void FixedUpdate()
    {
        if (currentState == PlayerStates.Idle)
        {
            rb.linearVelocity = Vector2.zero;
            canMove = true;
        }
        else if (currentState == PlayerStates.Walking)
        {
            rb.linearVelocity = currentSpeed * moveInput;
        }
        else if (currentState == PlayerStates.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
            canMove = false;
        }
    }

    public void Invis(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(ActivateInvis());
        }
    }

    public void SetNightSpeed()
    {
        isDay = false;
        if (isCleaning)
        {
            cleanSpeed = nightSpeed / 2;
            currentSpeed = cleanSpeed;
        }
        else currentSpeed = nightSpeed;
    }

    public void SetNormalSpeed()
    {
        isDay = true;
        if (isCleaning)
        {
            cleanSpeed = daySpeed / 2;
            currentSpeed = cleanSpeed;
        }
        else currentSpeed = daySpeed;
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
        if (canMove == false) return;

        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            lastMoveInput = moveInput;
            Vector2 animInput = isCleaning ? -moveInput : moveInput;
            animator.SetFloat("InputX", animInput.x);
            animator.SetFloat("InputY", animInput.y);
            ChangeState(PlayerStates.Walking);
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
            Vector2 lastAnimInput = isCleaning ? -lastMoveInput : lastMoveInput;
            animator.SetFloat("LastInputX", lastAnimInput.x);
            animator.SetFloat("LastInputY", lastAnimInput.y);
            ChangeState(PlayerStates.Idle);
        }
    }
    void AimRotation()
    {
        playerFacingTowards.up = lastMoveInput;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isKilling)
        {
            StartCoroutine(Kill());
        }
    }
    public void Clean(InputAction.CallbackContext context)
    {
        Collider2D Victim = Physics2D.OverlapCircle(attackPoint.position, attackRange, victims);
        if (Victim == null || isCleaning || !Victim.gameObject.GetComponent<Npc_Victims>().isDead) return;
        Destroy(Victim.gameObject);
        isCleaning = true;
        //Used variable just for keeping a track on the speeds
        cleanSpeed = currentSpeed / 2;
        currentSpeed = cleanSpeed;
        corpseInstance = Instantiate(Corpseprefab, Victim.transform.position, Quaternion.identity);
    }

    public void StopCleaning()
    {
        Destroy(corpseInstance);
        isCleaning = false;
        if (isDay) currentSpeed = daySpeed;
        else currentSpeed = nightSpeed;
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
        if (Victim != null && Victim.GetComponent<Npc_Victims>().isDead==false)
        { 
            Victim.GetComponent<Npc_Victims>().isDead = true;

            InsanityMeter.instance.ApplyInsanity(-15f);

            Victim.GetComponent<Npc_Victims>().anim.SetBool("isDead", true);
            AudioManager.instance.PlaySoundFx(eatingBones, transform, 1f);
            killingScreen.SetActive(true);
            killingScreen.GetComponentInChildren<Animator>().Play("KillingAnimation");
            ChangeState(PlayerStates.Attacking);
            isKilling = true;
            yield return new WaitForSeconds(attackTime);
            killingScreen.SetActive(false);
            isKilling = false;
            ChangeState(PlayerStates.Idle);
        }
    }


}
public enum PlayerStates { Idle, Walking, Attacking, Invisible};