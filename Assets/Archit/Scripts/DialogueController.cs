using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; }

    public TMP_Text dialogueTextBox, nameTextBox;
    public Image potraitImage;
    public GameObject dialoguePanel;
    public Transform choicePanel;
    public GameObject choiceButtonPrefab;


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

    public void GenerateChoices(DialogueChoice choiceDialogue, UnityEngine.Events.UnityAction onClick)
    {
        for(int i=1; i<choiceDialogue.choices.Length; i++)
        {
            GameObject choiceButton = Instantiate(choiceButtonPrefab, choicePanel);
            TMP_Text choiceButtonText = choiceButton.GetComponentInChildren<TMP_Text>();
            choiceButtonText.SetText(choiceDialogue.choices[i]);
            choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        }
    }

    public void ClearChoices()
    {
        foreach(Transform child in choicePanel)
        {
            Destroy(child.gameObject);
        }
    }
}
