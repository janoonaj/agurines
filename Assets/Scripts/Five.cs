using UnityEngine;
using System.Collections;

public class Five : MonoBehaviour {
    private const float MAX_X_SPEED = 5f;
    [HideInInspector] public Vector3 respawnPos;
    [HideInInspector] public Vector3 currentPos;
    [HideInInspector] public Vector3 lastPos;

    void Awake() {
        GetComponent<Renderer>().material.color = new Color(1, 1, 0.17f);
    }

    void Start() {
        currentPos = lastPos = respawnPos = transform.position;
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

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "OutOfScreenTrigger") {
            transform.position = respawnPos;
            stopSpeed();
            callCameraToCenter();
            GameObject.Find("Engine").GetComponent<ResetScene>().reset();
        }
        else if (col.gameObject.tag == "Respawn") {
            const float RESPAWN_MARGIN_Y_AXIS = 0.5f;
            Vector3 flagPos = col.transform.position;
            respawnPos = new Vector3(flagPos.x, flagPos.y + RESPAWN_MARGIN_Y_AXIS, 0f);
            Destroy(col.gameObject);            
        }
        else if (col.gameObject.tag == "LevelFinishedTrigger") {
            GameObject.Find("Engine").GetComponent<SceneController>().gotoNextLevel();
        }
        else if(col.gameObject.tag == "Collectible") {
            GameObject.Find("Engine").GetComponent<CollectiblePoints>().recollectPoint();
            Destroy(col.gameObject);
        }
    }

    private void stopSpeed() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    private void callCameraToCenter() {
        GameObject.Find("Main Camera").GetComponent<CameraMovement>().centerOn(respawnPos);
    }
}
