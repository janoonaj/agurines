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
    private GameObject player;

    void Start() {
        base.init();
        waitingTime = 0f;
        colliderTrigger = GetComponent<BoxCollider2D>();
        player = GameObject.Find("five");
        impulseSpeedVector = calculateImpulseForceVector();       
        stop();
    }

    override protected void onMovementFinished() {
        base.onMovementFinished();
        stop();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.name != "five") return;

        move();
        if (impulseSpeed > 0) {
            player.GetComponent<Five>().applySpeed(impulseSpeedVector);
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
