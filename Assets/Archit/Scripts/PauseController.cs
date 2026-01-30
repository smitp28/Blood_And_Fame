using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;
    public PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        playerInput.SwitchCurrentActionMap("UI");
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        playerInput.SwitchCurrentActionMap("Player");
    }
}
