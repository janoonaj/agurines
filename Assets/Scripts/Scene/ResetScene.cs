using UnityEngine;
using System.Collections;

public class ResetScene : MonoBehaviour {
    private GameObject[] fallingFloors;
    private GameObject[] impulseFloors;

    void Start () {
        fallingFloors = GameObject.FindGameObjectsWithTag("Fall");
        impulseFloors = GameObject.FindGameObjectsWithTag("ImpulseFloor");
	}
	
	public void reset() {
        foreach(GameObject fallingFloor in fallingFloors) {
            fallingFloor.GetComponent<FallFloor>().reset();
        }
        foreach (GameObject impulseFloor in impulseFloors) {
            impulseFloor.GetComponent<ImpulseFloor>().reset();
        }
    }
}
