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
    public Vector3 initialPos;
    public Quaternion initialRotation;

    void Start() {
        base.init();
        waitingTime = 0f;
        colliderTrigger = GetComponent<BoxCollider2D>();
        saveInitialTransformation();
        stop();
    }

    private int calculateNumMaxMovements() {
        if (comesBack) return 2;
        else return 1;
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
        activate(calculateNumMaxMovements());
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
        //If comesback does not need reset, 
        //it already came back to original position.
        if(comesBack == false) {
            transform.position = initialPos;
            transform.rotation = initialRotation;
            deactivate();
        }
    }
}
