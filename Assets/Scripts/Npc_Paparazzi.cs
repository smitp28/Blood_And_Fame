using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AdaptivePerformance.Editor;
public class Npc_Paparazzi : MonoBehaviour
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
    public GameObject marker11;
    public GameObject marker12;
    public GameObject marker13;
    public GameObject marker14;
    public GameObject marker15;
    public GameObject marker16;
    public GameObject marker17;
    public GameObject marker18;
    public GameObject marker19;
    public GameObject marker20;
    public GameObject marker;
    public Collider2D paparazzicoll;
    public LayerMask player;
    public FOV fieldOfView;
    private float cooldown = 1f;
    private float randomnum;
    private Vector3 personaloffset;
    //public Animator anim;
    public Vector3 vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        paparazzicoll = GetComponent<Collider2D>();
        agent.stoppingDistance = 0.1f;
        marker1 = GameObject.FindWithTag("M1");
        marker2 = GameObject.FindWithTag("M2");
        marker3 = GameObject.FindWithTag("M3");
        marker4 = GameObject.FindWithTag("M4");
        marker5 = GameObject.FindWithTag("M5");
        marker6 = GameObject.FindWithTag("M6");
        marker7 = GameObject.FindWithTag("M7");
        marker8 = GameObject.FindWithTag("M8");
        marker9 = GameObject.FindWithTag("M9");
        marker10 = GameObject.FindWithTag("M10");
        marker11 = GameObject.FindWithTag("M11");
        marker12 = GameObject.FindWithTag("M12");
        marker13 = GameObject.FindWithTag("M13");
        marker14 = GameObject.FindWithTag("M14");
        marker15 = GameObject.FindWithTag("M15");
        marker16 = GameObject.FindWithTag("M16");
        marker17 = GameObject.FindWithTag("M17");
        marker18 = GameObject.FindWithTag("M18");
        marker19 = GameObject.FindWithTag("M19");
        marker20 = GameObject.FindWithTag("M20");
        StartCoroutine(WaitAndMove());
        personaloffset = Random.insideUnitCircle.normalized * Random.Range(0.2f, 1f);
    }

    // Update is called once per frame
    public void Update()
    {
        fieldOfView.SetOrigin(transform.position);
        vel = agent.velocity;
        //anim.SetFloat("DirX", vel.x);
        //anim.SetFloat("DirY", vel.y);
        if (vel.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    IEnumerator WaitAndMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => fieldOfView.isCheckingcorpse == false);
            PickNewMarker();
            agent.isStopped = false;
            //anim.SetBool("isIdle", false);
            //anim.SetBool("isWalking", true);
            agent.SetDestination(targetpos.position + personaloffset);
            yield return new WaitUntil(() => !agent.pathPending || agent.hasPath);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f);
            //anim.SetFloat("LastDirX", vel.x);
            //anim.SetFloat("LastDirY", vel.y);
            agent.isStopped = true;
            //anim.SetBool("isIdle", true);
            //anim.SetBool("isWalking", false);
            yield return new WaitForSeconds(cooldown);
        }
    }

    public void PickNewMarker()
    {
        randomnum = Random.Range(0f, 1f);
        if (randomnum <= 0.05f)
        {
            marker = marker1;
        }
        else if (randomnum <= 0.1f)
        {
            marker = marker2;
        }
        else if (randomnum <= 0.15f)
        {
            marker = marker3;
        }
        else if (randomnum <= 0.2f)
        {
            marker = marker4;
        }
        else if (randomnum <= 0.25f)
        {
            marker = marker5;
        }
        else if (randomnum <= 0.3f)
        {
            marker = marker6;
        }
        else if (randomnum <= 0.35f)
        {
            marker = marker7;
        }
        else if (randomnum <= 0.4f)
        {
            marker = marker8;
        }
        else if (randomnum <= 0.45f)
        {
            marker = marker9;
        }
        else if (randomnum <= 0.5f)
        {
            marker = marker10;
        }
        else if (randomnum <= 0.55f)
        {
            marker = marker11;
        }
        else if (randomnum <= 0.6f)
        {
            marker = marker12;
        }
        else if (randomnum <= 0.65f)
        {
            marker = marker13;
        }
        else if (randomnum <= 0.7f)
        {
            marker = marker14;
        }
        else if (randomnum <= 0.75f)
        {
            marker = marker15;
        }
        else if (randomnum <= 0.8f)
        {
            marker = marker16;
        }
        else if (randomnum <= 0.85f)
        {
            marker = marker17;
        }
        else if (randomnum <= 0.9f)
        {
            marker = marker18;
        }
        else if (randomnum <= 0.95f)
        {
            marker = marker19;
        }
        else if (randomnum <= 1f)
        {
            marker = marker20;
        }
        targetpos = marker.transform;

    }

}
