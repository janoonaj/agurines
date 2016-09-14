using UnityEngine;
using System.Collections;

public class movementFloor : MonoBehaviour {

    public float count;
    private bool goingRight = true;
    private Vector3 startPos;
    public float speed;

    void Start() {
        startPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 nextPos;
        if(goingRight) {
            nextPos = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);
            if (nextPos.x > startPos.x + count) goingRight = false;

            
        } else {
            nextPos = new Vector3(transform.position.x - Time.deltaTime * speed, transform.position.y, transform.position.z);
            if (nextPos.x < startPos.x - count) goingRight = true;
        }


        transform.position = nextPos;

    }
}

