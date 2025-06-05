using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Throw(float force, Vector3 direction)
    {
        rb.useGravity = true;
        rb.AddForce(direction * force);
    }
}