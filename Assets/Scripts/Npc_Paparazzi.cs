using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
public class Npc_Paparazzi : MonoBehaviour
{
    public Transform targetpos;
    public NavMeshAgent agent;
    public GameObject[] markers;
    public float minSpeed = 2.0f;
    public float maxSpeed = 4.0f;
    public Collider2D paparazzicoll;
    public LayerMask player;
    public FOV fieldOfView;
    private int randomNum;
    private float cooldown = 1f;
    private Vector3 personaloffset;
    //public Animator anim;
    public Vector3 vel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        paparazzicoll = GetComponent<Collider2D>();
        agent.stoppingDistance = 0.1f;
        markers = GameObject.FindGameObjectsWithTag("Markers");
        StartCoroutine(WaitAndMove());
        personaloffset = Random.insideUnitCircle.normalized * Random.Range(0.2f, 1f);
    }

    // Update is called once per frame
    public void Update()
    {
        vel = agent.velocity;

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
            agent.SetDestination(targetpos.position + personaloffset);
            yield return new WaitUntil(() => !agent.pathPending || agent.hasPath);
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f);
            agent.isStopped = true;
            yield return new WaitForSeconds(cooldown);
        }
    }

    public void PickNewMarker()
    {
        randomNum = Random.Range(0, markers.Length);
        targetpos = markers[randomNum].transform;
    }

}
