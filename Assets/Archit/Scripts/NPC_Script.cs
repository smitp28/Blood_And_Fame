using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Script : MonoBehaviour, IInteractable
{
    public Dialogues dialogueData;
    public TMP_Text dialogueTextBox;
    public GameObject dialoguePanel;
    public Image potraitImage;
    public TMP_Text nameTextBox;
    public Button closeButton;
    private bool isTyping;
    private int dialogueIndex;
    private bool isDialogueActive;

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
        closeButton.interactable = false;
        isDialogueActive = true;
        dialogueIndex = 0;

        potraitImage.sprite = dialogueData.npcSprite;
        nameTextBox.SetText(dialogueData.name);

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }
    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueTextBox.SetText(dialogueData.dialogueLines[dialogueIndex]);
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
        dialogueTextBox.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueTextBox.text += letter;
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
        StopAllCoroutines();
        dialogueTextBox.SetText("");
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }
}
