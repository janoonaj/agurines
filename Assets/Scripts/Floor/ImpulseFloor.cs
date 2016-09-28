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
    public bool comesBack = true;
    private BoxCollider2D colliderTrigger;
    
    void Start() {
        base.init();
        waitingTime = 0f;
        if (comesBack) maxMovements = 2;
        else maxMovements = 1;
        colliderTrigger = GetComponent<BoxCollider2D>();
        stop();
    }

    override protected void onMovementFinished() {
        base.onMovementFinished();
        stop();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.name == "five") {
            move();    
        }
    }

    private void move() {
        enableMovement = true;
        colliderTrigger.enabled = false;
    }

    private void stop() {
        enableMovement = false;
        colliderTrigger.enabled = true;
    }
}
