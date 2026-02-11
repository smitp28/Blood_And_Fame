using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<string> completedQuestIDs = new List<string>();
    public AudioClip questCompleted_sfx;
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
        Rabbit livingTarget = null;
        Transform ownerNPC = null;

        foreach (QuestObjective objective in activeQuest.objectives)
        {
            if (objective.type == ObjectiveType.FindLiving)
            {
                livingTarget = LivingRegistry.GetLiving(objective.objectiveID);
            }

            if (objective.type == ObjectiveType.TalkNPC)
            {
                ownerNPC = NPC_Registry.GetNPC(objective.objectiveID);
            }
        }

        if (livingTarget != null && ownerNPC != null)
        {
            livingTarget.ReturnToOwner(ownerNPC);
        }
        completedQuestIDs.Add(activeQuest.QuestID);
        questUI.ClearObjectives(questUI.questPanel.transform.Find("QuestName").GetComponent<TMP_Text>(), questUI.questPanel.transform.Find("ObjectiveList").GetComponent<Transform>());
        AudioManager.instance.PlaySoundFx(questCompleted_sfx, transform, 1f);
        Debug.Log("Quest Finished: " + activeQuest.quest.questName);
        activeQuest = null;
    }
    public bool CheckBeforeObjective(int objectiveIndex)
    {
        for(int i=0; i<objectiveIndex; i++)
        {
            if (!activeQuest.objectives[i].IsCompleted) return false;
        }
        return true;
    }

    public void UpdateObjectiveProgress(int objectiveIndex, int amount)
    {
        if (activeQuest.objectives[objectiveIndex].currentAmount >= activeQuest.objectives[objectiveIndex].requiredAmount) return;
        activeQuest.objectives[objectiveIndex].currentAmount += amount;
    }
}