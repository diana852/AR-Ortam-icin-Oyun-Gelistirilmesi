using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeDetector : MonoBehaviour
{
    [SerializeField] private GameObject particleSystemPrefab;
    [SerializeField] private TMP_Text scoreText;

    [HideInInspector]
    public int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Increment the score
            score++;

            // Update the UI text to display the score
            scoreText.text = score.ToString();

            // Instantiate the particle system prefab
            GameObject particleSystemInstance = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);

            // Optionally, you can parent it to the CubeDetector or another GameObject
            particleSystemInstance.transform.parent = transform;

            // Destroy the particle system after 3 seconds
            Destroy(particleSystemInstance, .4f);
        }
    }
}
