using UnityEngine;

public class Dustbin : MonoBehaviour, IInteractable
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.instance;
    }
    bool IInteractable.CanInteract()
    {
        if(playerController.isCleaning)
        {
            return true;
        }
        return false;
    }
    void IInteractable.Interact()
    {
        //Play Animation
        
        playerController.StopCleaning();
    }
}
