using UnityEngine;

public class NPC_Status : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite notStartedSprite;
    [SerializeField] Sprite inProgressSprite;
    [SerializeField] Sprite completedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateStatusIcon(QuestState questState)
    {
        switch (questState) {
            case QuestState.NotStarted:
                spriteRenderer.sprite = notStartedSprite;
                break;
            case QuestState.InProgress:
                spriteRenderer.sprite = inProgressSprite;
                break;
            case QuestState.Completed:
                spriteRenderer.sprite = completedSprite;
                break;
        }
    }
}
