using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public GameObject objectiveTextPrefab;
    public GameObject questPanel;
    public Quest testQuest;
    private List<QuestObjective> objectives = new();

    private void Start()
    {
        for (int i = 0; i < testQuest.objectives.Count; i++) {
            objectives.Add(testQuest.objectives[i]);
        }
        UpdateQuestUI();
    }
    public void UpdateQuestUI()
    {
        TMP_Text questNameText = questPanel.transform.Find("QuestName").GetComponent<TMP_Text>();
        Transform objectiveList = questPanel.transform.Find("ObjectiveList").GetComponent<Transform>();
        foreach (Transform child in objectiveList)
        {
            Destroy(child.gameObject);
        }


        questNameText.text = testQuest.questName;
        foreach(var objective in objectives)
        {
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";
        }
    }
}
