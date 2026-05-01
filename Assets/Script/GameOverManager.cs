using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private static GameOverManager instance;

    void Awake()
    {
        instance = this;
        gameOverPanel.SetActive(false);
    }

    public static void TriggerGameOver()
    {
        instance.gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
