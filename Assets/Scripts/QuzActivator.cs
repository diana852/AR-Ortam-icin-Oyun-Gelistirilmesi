using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QuizActivator : MonoBehaviour
{
    public GameObject quizObject; // Inspector'dan QuizPrefab1'i buraya sürükle

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (quizObject != null && !quizObject.activeSelf)
            {
                quizObject.SetActive(true);
            }
        }

        DisableAllARPlanes();
    }

    private void DisableAllARPlanes()
    {

        ARPlane[] allPlanes = FindObjectsOfType<ARPlane>();
        foreach (ARPlane plane in allPlanes)
        {
            plane.gameObject.SetActive(false);
        }


        ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.enabled = false;
        }
        else
        {
            Debug.LogWarning("ARPlaneManager not found in the scene.");
        }
    }

}
