using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AdaptivePerformance.Editor;
public class Npc_Paparazzi : MonoBehaviour
{
    public Transform targetpos;
    NavMeshAgent agent;
    public GameObject marker1;
    public GameObject marker2;
    public GameObject marker3;
    public GameObject marker4;
    public GameObject marker;
    public Collider2D paparazzicoll;
    public LayerMask player;
    public FOV fieldOfView;
    private float cooldown = 1f;
    private float randomnum;
    private bool isWaiting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        paparazzicoll = GetComponent<Collider2D>();

        PickNewMarker();
        StartMoving();
    }

    // Update is called once per frame
    public void Update()
    {
       if (agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
        {
           StartCoroutine(WaitAndMove());
        }

        fieldOfView.SetOrigin(transform.position);
        Vector3 vel = agent.velocity;

        if (vel.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void StartMoving()
    {
        agent.speed = 3.5f;
        agent.SetDestination(targetpos.position);
    }

    public void StopMoving()
    { 
        agent.speed = 0;   
    }

    public void PickNewMarker()
    {
        randomnum = Random.Range(0f, 1f);
        if (randomnum < 0.25f)
        {
            marker = marker1;
        }
        else if (randomnum < 0.5f)
        {
            marker = marker2;
        }
        else if (randomnum < 0.75f)
        {
            marker = marker3;
        }
        else if (randomnum < 1)
        {
            marker = marker4;
        }
        targetpos = marker.transform;
    }

    IEnumerator WaitAndMove()
    {   
        isWaiting = true;
        StopMoving();
        yield return new WaitForSeconds(cooldown);
        PickNewMarker();
        StartMoving();
        isWaiting = false;
    }



}
