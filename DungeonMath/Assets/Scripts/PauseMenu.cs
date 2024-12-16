using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the UI Panel
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Use "Escape" or another key
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the menu
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(1); // Reload the current scene
    }

    public void RestartGame(){
        Time.timeScale = 1f; // Reset time scale
        GameManager.currentLevel = 1;
        SceneManager.LoadScene(0);
    }

}