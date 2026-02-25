using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Victim_Corpset : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform ownerTrans;
    //public int counter = 1;
    public int counterStop = 1;
    public int counter;
    public Vector3 targetpos;
    public Collider2D corpsecol;
    public Animator anim;
    public Vector3 offset;
    public Vector3 Direction;
    public float movespeed=5f;
    public float stopdist = 0.2f;
    private void Start()
    {
        counter = counterStop;
        corpsecol = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        ownerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();   
        offset = new Vector3(0f, -0.2f, 0f);
    }

    private void FixedUpdate()
    {
        targetpos = ownerTrans.position + offset;
        Direction = (targetpos - transform.position);
        
        if (Direction.magnitude > stopdist)
        {
            rb.linearVelocity = Direction.normalized * movespeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}