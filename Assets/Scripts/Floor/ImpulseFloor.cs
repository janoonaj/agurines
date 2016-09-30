using UnityEngine;
using System.Collections;
using System;


/*
 * This floor will move like MovementFloor, but, when it finishes, 
 * adds a force to the ball.
 * 
 * It's also triggered and only moves once.
 */

public class ImpulseFloor : MovementFloor {
    public float impulseSpeed;
    private Vector2 impulseSpeedVector;
    private BoxCollider2D colliderTrigger;
    private Vector3 initialPos;
    private Quaternion initialRotation;
    private GameObject player;

    void Start() {
        base.init();
        waitingTime = 0f;
        colliderTrigger = GetComponent<BoxCollider2D>();
        player = GameObject.Find("five");
        saveInitialTransformation();
        impulseSpeedVector = calculateImpulseForceVector();       
        stop();
    }

    override protected void onMovementFinished() {
        base.onMovementFinished();
        stop();
    }

    override protected void onFirstMovement() {
        /*if (impulseForce > 0)
            player.GetComponent<Rigidbody2D>().AddForce(impulseForceVector, ForceMode2D.Impulse);*/
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.name == "five") {
            move();
            if (impulseSpeed > 0) {
                player.GetComponent<Five>().applySpeed(impulseSpeedVector);

            }
        }
    }

    private void move() {
        activate();
        colliderTrigger.enabled = false;
    }

    private void stop() {
        enableMovement = false;
        colliderTrigger.enabled = true;
    }

    private void saveInitialTransformation() {
        initialPos = transform.position;
        initialRotation = transform.rotation;
    }

    public void reset() {
        transform.position = initialPos;
        transform.rotation = initialRotation;
        deactivate();
    }

    private Vector2 calculateImpulseForceVector() {
        if (direction.x > 0) {
            return new Vector2(impulseSpeed, 0);
        }
        else if (direction.x < 0) {
            return new Vector2(-impulseSpeed, 0);
        }
        else if (direction.y < 0) {
            return new Vector2(0, -impulseSpeed);
        }
        else if (direction.y > 0) {
            return new Vector2(0, impulseSpeed);
        }
        throw (new ArgumentNullException());
    }
}
