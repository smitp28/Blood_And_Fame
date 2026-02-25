using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void RestartGame()
    {
        // 1. Reset time immediately
        Time.timeScale = 1f;

        // 2. Switch the input map back BEFORE loading the scene
        if (PauseController.instance != null)
        {
            PauseController.instance.UnPause();
        }

        // 3. Load the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
