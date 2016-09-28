using UnityEngine;

/*
 * Class to count elapsed time.
 */ 

public class Clock {
    private float waitingTime;
    private float timePassed = 0f;

    public Clock(float waitingTime) {
        this.waitingTime = waitingTime;
    }

    /*
    * Update() should be called from Monobehaviuor object.
    * Returns true if time has passed.
    * */
    public bool update(float elapsedTime) {
        timePassed += elapsedTime;
        if(timePassed >= waitingTime) {
            timePassed = 0f;
            return true;
        }
        return false;
    }

}

