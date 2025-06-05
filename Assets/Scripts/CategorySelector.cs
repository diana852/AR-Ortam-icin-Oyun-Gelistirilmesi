using UnityEngine;
using UnityEngine.SceneManagement;

public class CategorySelector : MonoBehaviour
{
    public void SelectCategory(string categoryFileName)
    {
        GameData.SelectedCategoryFile = categoryFileName;
        SceneManager.LoadScene("StudyAR 2");
    }
}
