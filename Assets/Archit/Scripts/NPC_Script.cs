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
        PauseController.instance.Pause(); 
        closeButton.interactable = false;
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueUI.SetNPCInfo(dialogueData.name, dialogueData.npcSprite);
        dialogueUI.ShowDialogueUI(true);

        StartCoroutine(TypeLine());
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
            StartCoroutine(TypeLine());
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
            dialogueUI.GenerateChoiceButton(dialogueChoice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    public void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        StartCoroutine(TypeLine());
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
