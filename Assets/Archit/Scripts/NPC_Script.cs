using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Script : MonoBehaviour, IInteractable
{
    public Dialogues dialogueData;
    private DialogueController dialogueUI;
    public Button closeButton;
    private bool isTyping;
    private int dialogueIndex;
    private bool isDialogueActive;
    private enum QuestState { NotStarted, InProgress, Completed}
    private QuestState questState = QuestState.NotStarted;
    private void Start()
    {
        dialogueUI = DialogueController.instance;
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
        {
            return;
        }

        if (isDialogueActive) //if player is in hurry he presses 'e' again then it automatically starts printing next Line 
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }
    public void StartDialogue()
    {
        SyncQuestState();
        if(questState == QuestState.NotStarted) {dialogueIndex = 0;}
        if(questState == QuestState.InProgress) {dialogueIndex = dialogueData.questInProgressIndex;}
        if(questState == QuestState.Completed) {dialogueIndex = dialogueData.questCompletedIndex;}

        closeButton.interactable = false;
        isDialogueActive = true;

        dialogueUI.SetNPCInfo(dialogueData.name, dialogueData.npcSprite);
        dialogueUI.ShowDialogueUI(true);
        PauseController.instance.Pause(); 

        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void SyncQuestState()
    {
        if (dialogueData.quest == null) return;

        string questID = dialogueData.quest.questID;

        if (QuestController.instance.completedQuestIDs.Contains(questID))
        {
            questState = QuestState.Completed;
        }
        var active = QuestController.instance.activeQuest;
        if (active != null && active.QuestID == questID)
        {
            if (active.IsCompleted)
            {
                questState = QuestState.Completed;
                QuestController.instance.OnQuestCompleted();  
            }
            else
            {
                questState = QuestState.InProgress;
            }
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }


    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            if (dialogueData.dialogueLines.Length <= dialogueIndex + 1)
            {
                closeButton.interactable = true;
            }
        }

        dialogueUI.ClearChoices();

        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
        }

        foreach(DialogueChoice choice in dialogueData.dialogueChoices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(choice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length) {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    public void DisplayChoices(DialogueChoice dialogueChoice)
    {
        for (int i = 0; i < dialogueChoice.choices.Length; i++)
        {
            int nextIndex = dialogueChoice.nextDialogueIndex[i];
            bool givesQuest = dialogueChoice.givesQuest[i];
            dialogueUI.GenerateChoiceButton(dialogueChoice.choices[i], () => ChooseOption(nextIndex, givesQuest));
        }
    }

    public void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest)
        {
            QuestController.instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
        }
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueTextBox.text += letter);
            yield return new WaitForSecondsRealtime(dialogueData.typingSpeed);
        }
        if (dialogueData.dialogueLines.Length <= dialogueIndex + 1)
        {
            closeButton.interactable = true;
        }
        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        isDialogueActive = false;
        PauseController.instance.UnPause();
    }
}
