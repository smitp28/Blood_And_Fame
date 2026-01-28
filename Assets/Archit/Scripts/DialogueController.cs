using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }

    public TMP_Text dialogueTextBox, nameTextBox;
    public GameObject dialoguePanel;
    public Image potraitImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null) { instance = this; }
        else {Destroy(gameObject); }    
    }


    public void SetNPCInfo(string nameText, Sprite npcPotrait)
    {
        nameTextBox.text = nameText;
        potraitImage.sprite = npcPotrait;
    }

    public void ShowDialogueUI(bool boolean)
    {
        dialoguePanel.SetActive(boolean);
    }

    public void SetDialogueText(string dialogueText)
    {
        dialogueTextBox.text = dialogueText;
    }
}
