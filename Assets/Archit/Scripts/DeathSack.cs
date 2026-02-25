using UnityEngine;
using UnityEngine.AI;

public class DeathSack : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform playerTrans;
    public float moveSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.7f;
        agent.velocity = moveSpeed * new Vector3(1, 1, 0); 
    }
    void Update()
    {
        agent.SetDestination(playerTrans.position);
    }
}
