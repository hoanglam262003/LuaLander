using UnityEngine;
using UnityEngine.InputSystem;

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
}