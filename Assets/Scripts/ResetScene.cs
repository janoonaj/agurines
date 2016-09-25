using UnityEngine;
using System.Collections;

public class ResetScene : MonoBehaviour {
    private GameObject[] fallingFloors;

    void Start () {
        fallingFloors = GameObject.FindGameObjectsWithTag("Fall");
	}
	
	public void reset() {
        foreach(GameObject fallingFloor in fallingFloors) {
            fallingFloor.GetComponent<FallFloor>().reset();
        }
    }
}
