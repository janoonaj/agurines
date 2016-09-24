using UnityEngine;
using System.Collections;

public class FallFloor : MonoBehaviour {
    public Collider2D coll;

    void fall() {
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.IsTouching(coll) && other.name == "five")
            fall();
    }
}
