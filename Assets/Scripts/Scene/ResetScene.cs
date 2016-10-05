using UnityEngine;
using System.Collections;

public class ResetScene : MonoBehaviour {
    private GameObject[] fallingFloors;
    private GameObject[] impulseFloors;
    private SceneController sceneController;

    void Start () {
        fallingFloors = GameObject.FindGameObjectsWithTag("Fall");
        impulseFloors = GameObject.FindGameObjectsWithTag("ImpulseFloor");
        GameObject controller = GameObject.Find("Controller");
        if(controller != null)
            sceneController = controller.GetComponent<SceneController>();
    }
	
	public void reset() {
        foreach(GameObject fallingFloor in fallingFloors) {
            fallingFloor.GetComponent<FallFloor>().reset();
        }
        foreach (GameObject impulseFloor in impulseFloors) {
            impulseFloor.GetComponent<ImpulseFloor>().reset();
        }
        if (sceneController != null)
            sceneController.reset();
    }
}
