using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public GameObject objectiveTextPrefab;
    public GameObject questPanel;

    private List<QuestObjective> objectives = new();

    private void Start()
    {
        UpdateQuestUI();
    }
    public void UpdateQuestUI()
    {
        if (QuestController.instance == null) return;

        TMP_Text questNameText = questPanel.transform.Find("QuestName").GetComponent<TMP_Text>();
        Transform objectiveList = questPanel.transform.Find("ObjectiveList").GetComponent<Transform>();

        ClearObjectives(questNameText, objectiveList);

        if (QuestController.instance.activeQuest != null && QuestController.instance.activeQuest.quest != null)
        {
            questNameText.text = QuestController.instance.activeQuest.quest.questName;
            objectives = QuestController.instance.activeQuest.objectives;

            foreach (var objective in objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
            }
        }
        else
        {
            questNameText.text = "No active quests.";
        }
    }


    public void ClearObjectives(TMP_Text questName, Transform objectiveList)
    {
        questName.text = "No quests";
        foreach (Transform child in objectiveList)
        {
            Destroy(child.gameObject);
        }
    }
}
