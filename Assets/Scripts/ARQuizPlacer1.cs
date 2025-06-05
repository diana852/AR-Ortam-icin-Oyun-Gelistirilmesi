using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ARQuizPlacer1 : MonoBehaviour
{
    public GameObject quizPrefab;
    private GameObject placedObject;

    private ARRaycastManager raycastManager;

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null)
        {
            Debug.LogWarning("ARRaycastManager not found in the scene.");
        }
    }

    void Update()
    {
        if (placedObject != null) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            Vector2 touchPosition = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager != null && raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                Camera mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogWarning("Main Camera not found.");
                    return;
                }

                
                Vector3 placePosition = new Vector3(
                    hitPose.position.x,
                    hitPose.position.y,
                    mainCamera.transform.position.z + 1.5f
                );

                Quaternion placeRotation = Quaternion.LookRotation(mainCamera.transform.forward);
                placedObject = Instantiate(quizPrefab, placePosition, placeRotation);
                placedObject.transform.localScale = Vector3.one * 0.0033f;

                Debug.Log("Prefab spawned at X/Y from raycast, Z 1.5m forward: " + placePosition);
                DisableAllARPlanes();
            }
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
