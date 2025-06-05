using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderAndPanelManager : MonoBehaviour
{
    public static class SceneData
    {
        public static string targetPanel = "";
        public static string nestedPanel = "";
    }

    public void LoadSceneAndSetPanel(string sceneName, string panelName)
    {
        SceneData.targetPanel = panelName;
        SceneData.nestedPanel = "";
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAndSetNestedPanel(string sceneName, string parentPanelName, string childPanelName)
    {
        SceneData.targetPanel = parentPanelName;
        SceneData.nestedPanel = childPanelName;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            // Canvas içinde target paneli ara
            Transform targetTransform = obj.transform.Find(SceneData.targetPanel);
            if (targetTransform != null)
            {
                // Önce sadece parent paneli aç
                targetTransform.gameObject.SetActive(true);

                // Eğer nested panel isteniyorsa onu bul
                Transform finalTarget = targetTransform;
                if (!string.IsNullOrEmpty(SceneData.nestedPanel))
                {
                    Transform nestedTransform = targetTransform.Find(SceneData.nestedPanel);
                    if (nestedTransform != null)
                    {
                        nestedTransform.gameObject.SetActive(true);
                        finalTarget = nestedTransform;
                    }
                    else
                    {
                        Debug.LogWarning("Nested panel bulunamadı: " + SceneData.nestedPanel);
                    }
                }

                // Şimdi aynı canvas altındaki tüm diğer panelleri kapat
                Canvas canvas = finalTarget.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    foreach (Transform child in canvas.transform)
                    {
                        if (child != targetTransform)
                            child.gameObject.SetActive(false);
                    }
                }

                break;
            }
        }

        SceneData.targetPanel = "";
        SceneData.nestedPanel = "";
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadGamePanelMS()
    {
        LoadSceneAndSetPanel("GameUI_SP", "GamePanel");
    }

    public void LoadGameRoom()
    {
        LoadSceneAndSetNestedPanel("GameUI_SP", "GamePanel", "GameRoomPanel");
    }

    public void MonaLisaARGallery()
    {
        SceneManager.LoadScene("ARGalleryMonaLisa");
    }

    public void ScreamARGallery()
    {
        SceneManager.LoadScene("ARGalleryScream");
    }

    public void SunflowersARGallery()
    {
        SceneManager.LoadScene("ARGallerySunflowers");
    }
}
