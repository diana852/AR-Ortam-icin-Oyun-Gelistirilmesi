using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalScoreText;
    public GameObject gameOverUI; // Reference to the GameOver UI GameObject
    [SerializeField] private float timeRemaining = 10f;
    public CubeDetector cubeDetector;

    void Start()
    {
        // Invoke the UpdateTimer method every second, starting after 1 second
        InvokeRepeating("UpdateTimer", 1f, 1f);
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            // Update the time remaining
            timeRemaining--;

            // Convert seconds to minutes and seconds
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            // Update the TextMeshPro text
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            // Timer reached zero, activate the GameOver UI
            Debug.Log("Game Over!");
            gameOverUI.SetActive(true);
            finalScoreText.text = cubeDetector.score.ToString();
            Time.timeScale = 0f;
            

            // Stop the timer when it reaches zero
            CancelInvoke("UpdateTimer");
        }
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AR_Basketball");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
