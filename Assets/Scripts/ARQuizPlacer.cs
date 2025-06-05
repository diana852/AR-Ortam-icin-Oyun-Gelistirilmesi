using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class ARQuizPlacer : MonoBehaviour
{
    public GameObject quizPrefab;
    private GameObject placedObject;

    void Update()
    {
        if (placedObject != null) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {


            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("Main Camera not found.");
                return;
            }

            
            Vector3 placePosition = mainCamera.transform.position + mainCamera.transform.forward * 1.5f;
            Quaternion placeRotation = Quaternion.LookRotation(mainCamera.transform.forward);
          
            placedObject = Instantiate(quizPrefab, placePosition, placeRotation);

            placedObject.transform.localScale = Vector3.one * 0.0033f;

            Debug.Log("Prefab spawned in front of camera at " + placePosition);
            DisableAllARPlanes();
        }
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
