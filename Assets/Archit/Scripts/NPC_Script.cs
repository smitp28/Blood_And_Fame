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
        else if(++dialogueIndex < dialogueData.dialogueLines.Length){
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueTextBox.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        if (dialogueData.dialogueLines.Length <= dialogueIndex + 1)
        {
            closeButton.interactable = true;
        }
        isTyping = false;
        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        PauseController.instance.UnPause();
        StopAllCoroutines();
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        isDialogueActive = false;
    }
}
