using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<string> completedQuestIDs = new List<string>();
    public static QuestController instance { get; private set; }
    public QuestProgress activeQuest=null; // This is declared but NOT initialized yet
    [SerializeField] private QuestUI questUI;

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        if (questUI == null) questUI = FindFirstObjectByType<QuestUI>();
        activeQuest = null;
    }

    public bool IsQuestActive(string questID)
    {
        if (activeQuest == null || activeQuest.quest == null)
        {
            return false;
        }

        return activeQuest.QuestID == questID;
    }

    public bool IsQuestCompleted()
    {
        QuestProgress quest = activeQuest;
        return (quest != null && quest.IsCompleted);
    }
    public void UpdateUI()
    {
        if (questUI != null) questUI.UpdateQuestUI();
    }

    public void AcceptQuest(Quest quest)
    {
        if (quest == null) return;

        if (activeQuest != null)
        {
            string currentName = (activeQuest.quest != null) ? activeQuest.quest.questName : "Unknown Quest";
            Debug.LogWarning("Already have a quest: " + currentName);
            return;
        }

        activeQuest = new QuestProgress(quest);
        if (questUI != null) questUI.UpdateQuestUI();
    }

    public void OnQuestCompleted()
    {
        if (activeQuest == null) return;

        completedQuestIDs.Add(activeQuest.QuestID);
        questUI.ClearObjectives(questUI.questPanel.transform.Find("QuestName").GetComponent<TMP_Text>(), questUI.questPanel.transform.Find("ObjectiveList").GetComponent<Transform>());

        Debug.Log("Quest Finished: " + activeQuest.quest.questName);

        activeQuest = null;
    }
}