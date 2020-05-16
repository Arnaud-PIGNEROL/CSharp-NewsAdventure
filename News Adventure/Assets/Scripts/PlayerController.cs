using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public FixedJoystick fixedJoystick;
    public Rigidbody2D rb;
    private Vector2 moveVelocity;
    void Update()
    {
        Vector2 moveInput = new Vector2(fixedJoystick.Vertical, fixedJoystick.Horizontal);

        moveVelocity = moveInput.normalized * speed;
    }
    public void FixedUpdate()
    {
        Vector2 direction = new Vector3(fixedJoystick.Vertical, fixedJoystick.Horizontal);
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
