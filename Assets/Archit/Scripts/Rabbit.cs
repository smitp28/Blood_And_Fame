using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Rabbit : MonoBehaviour, IInteractable
{
    public Rigidbody2D rb;
    public float moveSpeed=5f;
    public float runTime=2f;
    public Transform ownerTrans;
    public int counter = 1;
    public NavMeshAgent myNavAgent;
    Vector2 randomDir;

    bool IInteractable.CanInteract()
    {
        return true;
    }
    private void Start()
    {
        myNavAgent = GetComponent<NavMeshAgent>();
        myNavAgent.updateRotation = false;
        myNavAgent.updateUpAxis = false;
        myNavAgent.stoppingDistance = 0.1f;
    }
    private void Update()
    {
        
    }
    void IInteractable.Interact()
    {
        if(counter >= 6)
        {
            return;
        }
        RunAway();
        counter++;
    }

    private void FixedUpdate()
    {
        if (counter >= 6) {
            myNavAgent.SetDestination(ownerTrans.position);
            myNavAgent.stoppingDistance = 2f;
        }
    }
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = transform.position;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    void RunAway()
    {
        myNavAgent.SetDestination(RandomNavmeshLocation(10f));
    }
    //IEnumerator RunAway()
    //{
    //    float randomX = Random.value;
    //    float randomY = Random.value;
    //    randomDir = new Vector2(randomX, randomY);
    //    rb.linearVelocity = randomDir * moveSpeed;
    //    yield return new WaitForSeconds(runTime);
    //    rb.linearVelocity = Vector2.zero;
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    StopCoroutine(RunAway());
    //}
}
