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
    public string dogID = "dog123";
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
            if (QuestController.instance.activeQuest == null)
            {
                Debug.Log("No quest active, can't pick up wallet logic-wise.");
                return;
            }
            QuestProgress quest = QuestController.instance.activeQuest;
            bool foundObjective = false;
            for (int i = 0; i < quest.objectives.Count; i++)
            {
                if (quest.objectives[i].objectiveID == dogID && quest.objectives[i].currentAmount < quest.objectives[i].requiredAmount)
                {
                    foundObjective = true;
                    QuestController.instance.UpdateObjectiveProgress(i, 1);
                }
            }
            if (foundObjective)
            {
                QuestController.instance.UpdateUI();
            }
            return;
        }
        RunAway();
        counter++;
    }

    private void FixedUpdate()
    {
        if (counter > 6) {
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
}
