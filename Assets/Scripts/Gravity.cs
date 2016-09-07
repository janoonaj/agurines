using UnityEngine;
using System.Collections;
using System;

public class Gravity : MonoBehaviour {
    
    private const float MAX_X_GRAVITY = 6f;

    void FixedUpdate() {

        float gravForce = 0f;

        if (Application.platform == RuntimePlatform.WindowsEditor) {
            float PC_GRAVITY = 0.4f;
            if (Input.GetKey(KeyCode.LeftArrow)) gravForce = -PC_GRAVITY;
            if (Input.GetKey(KeyCode.RightArrow)) gravForce = PC_GRAVITY;

            if (Input.GetKey(KeyCode.UpArrow)) {
                GameObject five = GameObject.Find("five");
                if (five != null) {
                    five.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                }
            }
        }

        if(Application.platform == RuntimePlatform.Android) {
            gravForce = Input.acceleration.x;
        }
        
        bool rotateLeft = false;
        if (gravForce < 0)
        {
            rotateLeft = true;
            gravForce = Mathf.Abs(gravForce);
        }

        Vector2 gravity = calculateGravity(gravForce, rotateLeft);
        gravity.x = limitXGravity(gravity.x, rotateLeft);
        gravity.y = removePositiveYGravity(gravity.y);

        Physics2D.gravity = new Vector3(gravity.x, gravity.y, 0f);
    }

    private Vector2 calculateGravity(float gravForce, bool rotateLeft) {
        Vector2 gravity = new Vector2(0f, 0f);
        float degrees = gravForce * 90;
        float rads = degrees * Mathf.PI / 180;
        gravity.x = Mathf.Sin(rads) * 9.81f;
        if (rotateLeft) gravity.x = -gravity.x;
        gravity.y = Mathf.Cos(rads) * -9.81f;

        return gravity;
    }

    private float limitXGravity(float accelX, bool rotateLeft) {
        if (rotateLeft)
            return Mathf.Max(-MAX_X_GRAVITY, accelX);
        return Mathf.Min(MAX_X_GRAVITY, accelX);
    }

    private float removePositiveYGravity(float accelY) {
        return Mathf.Min(0, accelY);
    }
}