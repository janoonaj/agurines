using UnityEngine;
using System.Collections;

public class FallFloor : MonoBehaviour {
    public Collider2D coll;
    public Vector3 pos;
    public Quaternion rotation;

    void Start() {
        pos = GetComponent<Rigidbody2D>().transform.position;
        rotation = GetComponent<Rigidbody2D>().transform.rotation;
    }

    void fall() {
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.IsTouching(coll) && other.name == "five")
            fall();
    }

    public void reset() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().transform.position = pos;
        GetComponent<Rigidbody2D>().transform.rotation = rotation;
    }
}
