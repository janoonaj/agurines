using UnityEngine;

public class lvl10Controller : MonoBehaviour, SceneController {

    private GameObject activavePlatform1;
    private GameObject activavePlatform2;
    public GameObject[] triggers;

    void Start() {
        activavePlatform1 = GameObject.Find("ActivablePlatform1");
        activavePlatform2 = GameObject.Find("ActivablePlatform2");
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.name != "five") return;

        foreach (GameObject trigger in triggers) {
            if (col.IsTouching(trigger.GetComponent<BoxCollider2D>()) == false)
                continue;
            if (trigger.name == "TriggerPlatform1") {
                activavePlatform1.GetComponent<MovementFloor>().enableMovement = true;
            }
        }
    }



    public void buttonPush(GameObject button) {
        
    }

    public void reset() {
        activavePlatform1.GetComponent<MovementFloor>().enableMovement = false;
        activavePlatform1.GetComponent<MovementFloor>().reset();
        activavePlatform2.GetComponent<MovementFloor>().enableMovement = false;
        activavePlatform2.GetComponent<MovementFloor>().reset();
    }
}
