using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogues", menuName = "NPC Dialogue")]
public class Dialogues : ScriptableObject
{
    public string npcName;
    public string npcID;
    public Sprite npcSprite;
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public bool[] endDialogueLines;

    public DialogueChoice[] dialogueChoices;

    public int questInProgressIndex;
    public int questCompletedIndex;
    public Quest quest;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public int[] nextDialogueIndex;
    public string[] choices;
    public bool[] givesQuest;
}