using UnityEngine;
using System.Collections;

public class MovementFloor : MonoBehaviour {
    
    public float speed;
    public Vector3 finalPos;
    public float waitingTime;
    private Vector3 direction;
    private Vector3 startPos;
    private bool waiting = false;
    private float waitingTimeCount = 0;
    private AreaEffector2D effector;


    void Start() {
        startPos = transform.position;
        direction = finalPos - startPos;
        effector = GetComponentInChildren<AreaEffector2D>();
    }
	
	void Update () {
        updateWaitingTime();
        if (waiting) return;

        Vector3 nextPos;
        nextPos = new Vector3(transform.position.x + Time.deltaTime * speed * direction.x,
                            transform.position.y + Time.deltaTime * speed * direction.y, 
                            transform.position.z);
        
        if(platformArrived(nextPos)) {
            switchDirection();
            stopForce();          
            waiting = true;
        }

        transform.position = nextPos;

    }

    private bool platformArrived(Vector3 nextPos) {
        if (direction.x > 0) {
            if (nextPos.x >= finalPos.x) return true;
        }
        else if (direction.x < 0) {
            if (nextPos.x <= finalPos.x) return true;
        }
        else if (direction.y < 0) {
            if (nextPos.y <= finalPos.y) return true;
        }
        else if (direction.y > 0) {
            if (nextPos.y >= finalPos.y) return true;
        }
        return false;
    }

    private void switchDirection() {
        Vector3 temp = startPos;
        startPos = finalPos;
        finalPos = temp;
        direction = finalPos - startPos;
    }

    private void updateWaitingTime() {
        if (waiting == false) return;
        this.waitingTimeCount += Time.deltaTime;
        if(waitingTimeCount >= waitingTime) {
            waitingTimeCount = 0;
            waiting = false;
            startForce();
        }
    }

    /*
     * Control the ball on a platform, when its moving, is very difficult for the player.
     * When the platform moves add a fake force, with the direction of the movement,
     * to make it easier.
     */
    private void startForce() {
        const float FAKE_FORCE = 1f;
        effector.forceAngle = Vector3.Angle(Vector3.right, direction);
        effector.forceMagnitude = FAKE_FORCE;
    }

    private void stopForce() {
        effector.forceMagnitude = 0f;
    }
}

