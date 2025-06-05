using UnityEngine;

public class Player : MonoBehaviour
{
    public Ball ballPrefab;  // Reference to the ball prefab
    public GameObject playerCamera;

    public float ballDistancefront = 2f;
    public float ballDistancevertical = 2f;
    public float ballThrowingforce = 550f;
    private Rigidbody rb;
    private Ball currentBall;  // Reference to the currently held ball


    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBall != null)
        {
            currentBall.transform.position = playerCamera.transform.position + playerCamera.transform.forward * ballDistancefront + playerCamera.transform.up * ballDistancevertical;

            HandleInput();
        }
    }


    void HandleInput()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            ThrowBall();
        }

        // Check for touch input
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                ThrowBall();
            }
        }
    }

    void ThrowBall()
    {
        // Check if currentBall is null before trying to access its properties
        if (currentBall != null)
        {
            currentBall.Throw(ballThrowingforce, playerCamera.transform.forward);

            // Check if rb is null before trying to modify its properties
            if (rb != null)
            {
                rb.useGravity = true;
                rb.freezeRotation = false;
            }
            Destroy(currentBall.gameObject, 5f);
            currentBall = null;

            // Invoke the SpawnNewBall method after a delay
            Invoke("SpawnNewBall", 2f);
        }
    }

    void SpawnNewBall()
    {
        // Instantiate a new ball
        Ball newBall = Instantiate(ballPrefab, playerCamera.transform.position + playerCamera.transform.forward * ballDistancefront + playerCamera.transform.up * ballDistancevertical, Quaternion.identity);
        rb = newBall.GetComponent<Rigidbody>();
        rb.useGravity = false;  // Disable gravity for the new ball
        rb.freezeRotation = true;  // Freeze rotation for the new ball

        currentBall = newBall;  // Set the newly instantiated ball as the current ball

        
    }

    public void OnButtonClick()
    {
        ThrowBall() ;
    }
}
