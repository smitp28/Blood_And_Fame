using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
public class Npc_Victims : MonoBehaviour, IInteractable
{
    public Transform targetpos;
    public GameObject[] markers;
    public GameObject marker;
    public NavMeshAgent agent;
    public Collider2D paparazzicoll;
    public LayerMask player;
    private float cooldown = 1f;
    private float randomnum;
    private Vector3 personaloffset;
    public bool isDead;
    public bool hasBeenscanned;
    public float deadTimer;
    public Animator anim;
    Vector3 vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        paparazzicoll = GetComponent<Collider2D>();
        agent.stoppingDistance = 0.1f;
        markers = new GameObject[50];
        for (int i = 0; i < markers.Length; i++) 
        { 
            markers[i] = GameObject.FindWithTag("M"+(i+1)); 
        }

        anim = GetComponent<Animator>();
        StartCoroutine(WaitAndMove());
        personaloffset = Random.insideUnitCircle.normalized * Random.Range(0.2f, 1f);
        isDead = false;
        deadTimer = 30f;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isDead)
        {
            agent.velocity = Vector3.zero;
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isDead", true);
            if (!hasBeenscanned)
            {
                deadTimer -= Time.deltaTime;

                if (deadTimer <= 0f)
                {
                    Destroy(gameObject);
                }
            }
         return;
        }
        vel = agent.velocity;
        anim.SetFloat("DirX", vel.x);
        anim.SetFloat("DirY", vel.y);
    }

    IEnumerator WaitAndMove()
    {
        while (!isDead)
        {
            PickNewMarker();
            agent.isStopped = false;
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);
            agent.SetDestination(targetpos.position + personaloffset);
            yield return new WaitUntil(() => !agent.pathPending || agent.hasPath);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f);
            anim.SetFloat("LastDirX", vel.x);
            anim.SetFloat("LastDirY", vel.y);
            agent.isStopped = true;
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            yield return new WaitForSeconds(cooldown);
        }
    }

    public void PickNewMarker()
    {
        randomnum = Random.Range(0f, 1f);
        for (int i = 0; i < markers.Length; i++)
        {
            if (randomnum <= (i + 1f) / markers.Length)
            {
                marker = markers[i];
                break;
            }
        }
        targetpos = marker.transform;
    }

    public void Death()
    {
        if (!isDead)
        { return; }
    }

    bool IInteractable.CanInteract()
    {
        return isDead;
    }

    void IInteractable.Interact()
    {
        
    }
}
