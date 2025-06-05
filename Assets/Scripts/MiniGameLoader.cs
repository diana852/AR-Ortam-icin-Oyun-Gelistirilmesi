using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameLoader : MonoBehaviour
{
    public void MonaLisaPuzzle()
    {
        SceneManager.LoadScene("PuzzleMonaLisa");
    }

    public void ScreamPuzzle()
    {
        SceneManager.LoadScene("PuzzleScream");
    }

    public void SunflowersPuzzle()
    {
        SceneManager.LoadScene("PuzzleSunflowers");
    }

    public void MemoryCard()
    {
        SceneManager.LoadScene("MemoryCard_UI");
    }

    public void Basketball()
    {
        Time.timeScale = 1f;    
        SceneManager.LoadScene("AR_Basketball");
    }
}
