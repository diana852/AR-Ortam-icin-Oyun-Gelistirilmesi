using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator animator;

    private float targetRotationY = 180f;           
    public float rotationSpeed = 360f;              // Derece/sn


    public void SetSadState(bool isSad)
    {
        animator.SetBool("isSad", isSad);
    }

    public void RunRight()
    {
        SetTargetYRotation(135f);
        animator.SetBool("isRunning", true);
        Invoke("StopRunning",0.65f);
    }

    public void RunLeft()
    {
        SetTargetYRotation(225f);
        animator.SetBool("isRunning", true);
        Invoke("StopRunning", 0.65f);
    }

    public void Jump()
    {
        animator.SetTrigger("isJumping");
    }

    public void Dance()
    {
        animator.SetTrigger("isDancing");
    }

    private void StopRunning()
    {
        animator.SetBool("isRunning", false);
        SetTargetYRotation(180f); // Idle pozisyonu
    }

    private void SetTargetYRotation(float y)
    {
        targetRotationY = y;
    }

    void Update()
    {
        // Mevcut rotasyon
        Quaternion currentRotation = transform.rotation;

        // Hedef rotasyon (sadece Y ekseni)
        Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);

        
        transform.rotation = Quaternion.RotateTowards(
            currentRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
