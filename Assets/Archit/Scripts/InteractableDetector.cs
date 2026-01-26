using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableDetector : MonoBehaviour
{

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }
    private IInteractable interactableInRange = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Interactable NOT Found");
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            Debug.Log("Interactable Found");
            interactableInRange = interactable;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable) && interactable==interactableInRange)
        {
            Debug.Log("Interactable Lost");
            interactableInRange = null;
        }
    }
}
