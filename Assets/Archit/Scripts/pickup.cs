using System.Threading.Tasks;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public string itemID="Wallet123";
    public QuestUI questUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Is this done?");
        if (collision.CompareTag("Player"))
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
                if(quest.objectives[i].objectiveID == itemID)
                {
                    foundObjective = true;  
                    quest.objectives[i].currentAmount++;
                }
            }
            if (foundObjective)
            {
                QuestController.instance.UpdateUI();
                gameObject.SetActive(false);
            }
        }
    }
}
