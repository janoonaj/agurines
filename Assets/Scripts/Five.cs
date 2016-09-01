using UnityEngine;
using System.Collections;

public class Five : MonoBehaviour {
    private const float MAX_X_SPEED = 5f;
    public Vector3 currentPos;
    public Vector3 lastPos;

    void Start() {
        currentPos = lastPos = transform.position;
    }

    void FixedUpdate() {
        limitXSpeed();
        lastPos = currentPos;
        currentPos = transform.position;
    }

    private void limitXSpeed() {
        float xSpeed = GetComponent<Rigidbody2D>().velocity.x;
        float symbol = (xSpeed > 0) ? 1 : -1;
        xSpeed = Mathf.Abs(xSpeed);
        xSpeed = Mathf.Min(MAX_X_SPEED, xSpeed);
        xSpeed *= symbol;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }
}
