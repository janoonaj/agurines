using UnityEngine;
using System.Collections;

public class MovementFloor : MonoBehaviour, IActivable {

    /*
     * TODO: needs massive refactor.
     * Forces and movement shouldn't be on the same class. ImpulseFloor is extending this
     * class and, also, implementing its own logic with forces.
     */ 
    
    public float speed;
    public Vector3 finalPos;
    public float waitingTime;
    public bool enableMovement = true;
    public int maxMovements; //Movements the platform will do, 0 means infinite movement
    protected Clock clock;
    protected Vector3 direction;
    protected Vector3 startPos;
    protected bool waiting = false;
    protected float waitingTimeCount = 0;
    protected AreaEffector2D effector;
    protected int movementsCount = 0;    
    private InitialConfig initialConfig;

    void Start() {
        init(); 
    }

    protected void init() {
        startPos = transform.position;
        calculateDirection();
        effector = GetComponentInChildren<AreaEffector2D>();
        clock = new Clock(waitingTime);
        initialConfig = saveInitialTransformation();
    }
	
	void Update () {
        if (enableMovement == false) return;
        updateWaitingTime();
        if (waiting) return;

        Vector3 nextPos;
        nextPos = new Vector3(transform.position.x + Time.deltaTime * speed * direction.x,
                            transform.position.y + Time.deltaTime * speed * direction.y, 
                            transform.position.z);

        
        if(platformArrived(nextPos)) {
            transform.position = finalPos;
            switchDirection();
            stopForce();
            movementsCount++;
            if(maxMovements > 0 && movementsCount >= maxMovements) {
                onMovementFinished();
            }       
            else
                waiting = true;
        } else {
            transform.position = nextPos;
        }
    }

    public void activate() {
        enableMovement = true;
    }

    protected void deactivate() {
        enableMovement = false;
        movementsCount = 0;
    }

    public void reset() {
        transform.position = startPos = initialConfig.initialPos;
        transform.rotation = initialConfig.initialRotation;
        finalPos = initialConfig.finalPos;
        calculateDirection();
        clock = new Clock(waitingTime);
        deactivate();
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
        calculateDirection();
    }

    /*
     * "Normalize" function does return rubbish sometimes.
     * Just enable 0 or 1. Only vertical or horizontal movement
     * allowed, not both.
     */ 
    private void calculateDirection() {
        direction = Vector3.Normalize(finalPos - startPos);
        if (Mathf.Approximately(direction.x, 1f) == false &&
            Mathf.Approximately(direction.x, -1f) == false)
            direction.x = 0;
        if (Mathf.Approximately(direction.y, 1f) == false &&
            Mathf.Approximately(direction.y, -1f) == false)
            direction.y = 0;

       /* if ((direction.x != 0f && direction.y != 0) ||
            (direction.x == 0f && direction.y == 0))
            throw (new System.Exception(name + " tries diagonal movement"));*/
    }

    private void updateWaitingTime() {
        if (waiting == false) return;
        if(clock.update(Time.deltaTime))
        {
            waitingTimeCount = 0;
            waiting = false;
            startForce();
        }
    }

    protected virtual void onMovementFinished() {
        deactivate();
    }

    /*
     * Control the ball on a platform, when its moving, is very difficult for the player.
     * When the platform moves add a fake force, with the direction of the movement,
     * to make it easier.
     */
    private void startForce() {
        if (!effector) return;
        float FAKE_FORCE = speed * 3.333f;
        effector.forceAngle = Vector3.Angle(Vector3.right, direction);
        effector.forceMagnitude = FAKE_FORCE;
    }

    private void stopForce() {
        if (!effector) return;
        effector.forceMagnitude = 0f;
    }

    private InitialConfig saveInitialTransformation() {
        InitialConfig config = new InitialConfig();
        config.initialPos = transform.position;
        config.initialRotation = transform.rotation;
        config.finalPos = finalPos;
        return config;
    }
}

class InitialConfig {
    public Vector3 initialPos;
    public Quaternion initialRotation;
    public Vector3 finalPos;
}

