using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(1); 
    }

    public void RestartGame(){
        Time.timeScale = 1f; 
        GameManager.currentLevel = 1;
        SceneManager.LoadScene(0);
    }

}