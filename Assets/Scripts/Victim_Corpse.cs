using UnityEngine;

public class Victim_Corpse : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform ownerTrans;
    public int counterStop = 1;
    public int counter;
    public Vector3 targetpos;
    public Collider2D corpsecol;
    public Animator anim;
    public Vector3 Direction;
    public float movespeed = 10f;
    public float stopdist = 0.2f;

    private void Start()
    {
        counter = counterStop;
        corpsecol = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        ownerTrans = GameObject.FindGameObjectWithTag("CorpseParent").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        targetpos = ownerTrans.position;
        Direction = targetpos - transform.position;
        Direction.z = 0f;

        float distance = Direction.magnitude;

        if (distance > stopdist)
        {
            // Only update rotation when moving so it doesn't snap weirdly when stopped
            transform.up = Direction;

            // Calculate the exact distance left to travel until we hit the stopdist barrier
            float distanceToCover = distance - stopdist;

            // Calculate how much speed is needed to cover that exact distance in one frame
            float exactSpeedNeeded = distanceToCover / Time.fixedDeltaTime;

            // Use our movespeed, UNLESS the exact speed needed is smaller (meaning we are very close)
            float appliedSpeed = Mathf.Min(movespeed, exactSpeedNeeded);

            rb.linearVelocity = Direction.normalized * appliedSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}