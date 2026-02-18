using UnityEngine;

public class NPC_Status : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateStatusIcon(QuestState questState)
    {
        switch (questState) {
            case QuestState.NotStarted:
                spriteRenderer.color = Color.red;
                break;
            case QuestState.InProgress:
                spriteRenderer.color = Color.yellow;
                break;
            case QuestState.Completed:
                spriteRenderer.color = Color.green;
                break;
        }
    }
}
