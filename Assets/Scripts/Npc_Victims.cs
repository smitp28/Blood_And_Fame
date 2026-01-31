using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AdaptivePerformance.Editor;
public class Npc_Victims : MonoBehaviour
{
    public Transform targetpos;
    public NavMeshAgent agent;
    public GameObject marker1;
    public GameObject marker2;
    public GameObject marker3;
    public GameObject marker4;
    public GameObject marker5;
    public GameObject marker6;
    public GameObject marker7;
    public GameObject marker8;
    public GameObject marker9;
    public GameObject marker10;
    public GameObject marker;
    public Collider2D paparazzicoll;
    public LayerMask player;
    private float cooldown = 1f;
    private float randomnum;
    private Vector3 personaloffset;
    public bool isDead;
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
        marker1 = GameObject.FindWithTag("VM1");
        marker2 = GameObject.FindWithTag("VM2");
        marker3 = GameObject.FindWithTag("VM3");
        marker4 = GameObject.FindWithTag("VM4");
        marker5 = GameObject.FindWithTag("VM5");
        marker6 = GameObject.FindWithTag("VM6");
        marker7 = GameObject.FindWithTag("VM7");
        marker8 = GameObject.FindWithTag("VM8");
        marker9 = GameObject.FindWithTag("VM9");
        marker10 = GameObject.FindWithTag("VM10");
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
        if (randomnum <= 0.1f)
        {
            marker = marker1;
        }
        else if (randomnum <= 0.2f)
        {
            marker = marker2;
        }
        else if (randomnum <= 0.3f)
        {
            marker = marker3;
        }
        else if (randomnum <= 0.4f)
        {
            marker = marker4;
        }
        else if (randomnum <= 0.5f)
        {
            marker = marker5;
        }
        else if (randomnum <= 0.6f)
        {
            marker = marker6;
        }
        else if (randomnum <= 0.7f)
        {
            marker = marker7;
        }
        else if (randomnum <= 0.8f)
        {
            marker = marker8;
        }
        else if (randomnum <= 0.9f)
        {
            marker = marker9;
        }
        else if (randomnum <= 1f)
        {
            marker = marker10;
        }
        targetpos = marker.transform;

    }

    public void Death()
    {
        if (!isDead)
        { return; }
    }


}
