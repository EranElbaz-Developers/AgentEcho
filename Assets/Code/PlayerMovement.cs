using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public float velocitySensitivity;
    public float rotationSensitivity;
    private float speedPercent;

    public float SpeedPercent
    {
        get => speedPercent;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Rotation()
    {
        float rotation_z = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotation_z += rotationSensitivity;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotation_z -= rotationSensitivity;
        }

        rb.angularVelocity = 0;
        if (rotation_z != 0)
        {
            rb.SetRotation(rb.rotation + rotation_z);
        }
    }

    private void Movement()
    {
        Vector2 velocity = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            var direction3d = -transform.right;
            var direction = new Vector2(direction3d.x, direction3d.y);
            velocity += direction * velocitySensitivity;
        }

        if (Input.GetKey(KeyCode.D))
        {
            var direction3d = transform.right;
            var direction = new Vector2(direction3d.x, direction3d.y);
            velocity += direction * velocitySensitivity;
        }

        if (Input.GetKey(KeyCode.W))
        {
            var direction3d = transform.up;
            var direction = new Vector2(direction3d.x, direction3d.y);
            velocity += direction * velocitySensitivity;
        }

        if (Input.GetKey(KeyCode.S))
        {
            var direction3d = -transform.up;
            var direction = new Vector2(direction3d.x, direction3d.y);
            velocity += direction * velocitySensitivity;
        }

        if (velocity.magnitude != 0)
        {
            speedPercent = 1;
        }
        else
        {
            speedPercent = 0;
        }
        rb.velocity = velocity;
    }
}