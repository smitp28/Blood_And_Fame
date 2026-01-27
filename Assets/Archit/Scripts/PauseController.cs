using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;
    public PlayerInput playerInput;
    private void Awake()
    {
        instance = this;
    }

    public void Pause()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
    public void UnPause()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }
}
