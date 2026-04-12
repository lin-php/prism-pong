using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameManager gameManager;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        } 
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pausePanel.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }
    public void RestartGame()
    {
        ResumeGame();
        gameManager.RestartGame();
    }
}
