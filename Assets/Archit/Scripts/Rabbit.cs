using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Rabbit : MonoBehaviour, IInteractable
{
    public Rigidbody2D rb;
    public Transform ownerTrans;
    public int counter = 1;
    public int counterStop = 6;
    public string dogID = "dog123";
    public NavMeshAgent myNavAgent;
    public Animator anim;

    bool IInteractable.CanInteract()
    {
        return true;
    }
    private void Start()
    {
        myNavAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        myNavAgent.updateRotation = false;
        myNavAgent.updateUpAxis = false;
        myNavAgent.stoppingDistance = 0.1f;
    }
    void IInteractable.Interact()
    {
        if(counter == counterStop)
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

    private void Update()
    {
        if (counter == 6) {
            myNavAgent.SetDestination(ownerTrans.position);
            myNavAgent.stoppingDistance = 2f;
        }
        if (anim != null)
        {
            Vector3 vel = myNavAgent.velocity;
            if (myNavAgent.velocity.magnitude <= 0.2f)
            {
                anim.SetFloat("LastDirX", vel.x);
                anim.SetFloat("LastDirY", vel.y);
                anim.SetBool("isIdle", true);
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetFloat("DirX", vel.x);
                anim.SetFloat("DirY", vel.y);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
            }
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

    public void ReturnToOwner(Transform owner)
    {
        counter = counterStop; // stop runaway logic

        myNavAgent.stoppingDistance = 1.5f;
        myNavAgent.SetDestination(owner.position);

        // Optional: disable interaction
        this.enabled = false;
    }

    private void OnEnable()
    {
        LivingRegistry.Register(dogID, this);
    }

    private void OnDisable()
    {
        LivingRegistry.Unregister(dogID);
    }
}
