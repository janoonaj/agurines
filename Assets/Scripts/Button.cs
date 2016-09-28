using UnityEngine;
using System.Collections;
using System;

public class Button : MonoBehaviour {
    private const float speed = 3f;
    public int angleDirection;
    public GameObject platform;
    private Vector3 finalPos;
    private Vector3 direction;
    private Vector3 startPos;
    private bool isMoving = false;
    private int movements = 0;
    private const float waitingTime = 1;
    private Clock clock;
    private bool isSleeping = false;


    void Start() {
        startPos = finalPos = transform.position;
        float distance = 0.1f;
        switch(angleDirection) {
            case 0: finalPos.x += distance; break;
            case 90: finalPos.y += distance;break;
            case 180: finalPos.x -= distance;break;
            case 270: finalPos.y -= distance;break;
            default:
                throw new ArgumentException("Invalid angle direction");
        }
        direction = Vector3.Normalize(finalPos - startPos);
    }

    void Update() {
        updateWaitingTime();
        if (isMoving == false) return;

        Vector3 nextPos;
        nextPos = new Vector3(transform.position.x + Time.deltaTime * speed * direction.x,
                            transform.position.y + Time.deltaTime * speed * direction.y,
                            transform.position.z);

        if (platformArrived(nextPos)) {
            transform.position = finalPos;
            movements++;
            switchDirection();
            if (movements == 2) {
                stopMovement();
            }
        } else {
            transform.position = nextPos;
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "five" && isMoving == false && isSleeping == false) {
            isMoving = true;
            platform.GetComponent<MovementFloor>().activate(2);
        }
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
        direction = Vector3.Normalize(finalPos - startPos);
    }

    private void stopMovement() {
        movements = 0;
        isMoving = false;
        isSleeping = true;
        clock = new Clock(waitingTime);
    }

    private void updateWaitingTime() {
        if (clock != null && clock.update(Time.deltaTime)) {
            clock = null;
            isSleeping = false;
        }
    }
}