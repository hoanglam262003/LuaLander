using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Lander : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 700f;
    public float turnLeftSpeed = 100f;
    public float turnRightSpeed = -100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            rb.AddForce(transform.up * Time.deltaTime * force);
        }
        else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            rb.AddTorque(turnLeftSpeed * Time.deltaTime);
        }
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            rb.AddTorque(turnRightSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the terrain!");
            return;
        }
        float softLanding = 4f;
        float relativeVelocity = collision.relativeVelocity.magnitude;
        if (relativeVelocity > softLanding)
        {
            Debug.Log("You crashed!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.9f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed bad angle!");
            return;
        }
        Debug.Log("Landed successfully!");

        float maxScoreLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreLandingAngle;

        float maxScoreLandingSpeed = 100f;
        float landingSpeedScore = (softLanding - relativeVelocity) * maxScoreLandingSpeed;
        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);
    }
}