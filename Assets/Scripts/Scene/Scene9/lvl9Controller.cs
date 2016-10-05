using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class lvl9Controller : MonoBehaviour, SceneController {
    public GameObject ballPrefab;
    public GameObject elevatorPrefab;
    public GameObject[] triggers;
    private GameObject ball1;
    private Vector3 BALL1_POS = new Vector3(0.39f, 3.44f, 0f);
    private Vector3 BALL2_POS = new Vector3(4.77f, -11.15f, 0f);
    private GameObject ball2;
    private GameObject button2; //Keep track to activate or deactivate the button
    private GameObject button3; 
    private GameObject elevator;
    private Dictionary<String, GameObject> walls;


    void Start () {
        ball1 = (GameObject) Instantiate(ballPrefab, BALL1_POS, Quaternion.identity);
        ball2 = (GameObject) Instantiate(ballPrefab, BALL2_POS, Quaternion.identity);
        ball2.SetActive(false);
        button2 = GameObject.Find("button2");
        button3 = GameObject.Find("button3");
        walls = new Dictionary<String, GameObject>();
        int numWallsOnScene = 11;
        for(int i = 1; i <= numWallsOnScene; i++) {
            walls["wall" + i] = GameObject.Find("wall" + i);
        }
        walls["platform"] = GameObject.Find("platform");
    }

    public void buttonPush(GameObject button) {
        if(button.name == "button1") {
            ball1.GetComponent<Five>().freeze();
        } 
        else if (button.name == "button2") {
            button2.SetActive(false);
        }
        else if (button.name == "button3") {
            button3.SetActive(false);
            elevator = createElevator();
        }
        else if (button.name == "button4") {
            ball2.GetComponent<Five>().freeze();
        }
    }

    public void reset() {        
        ball1.transform.position = BALL1_POS;
        ball1.GetComponent<Five>().unfreeze();
        ball2.transform.position = BALL2_POS;
        ball2.GetComponent<Five>().unfreeze();
        ball2.SetActive(false);
        button2.SetActive(true);
        button3.SetActive(true);
        if (elevator != null) {
            Destroy(elevator);
        }
        foreach (GameObject trigger in triggers) {
            trigger.SetActive(true);
        }
        foreach (var wall in walls.Values) {
            wall.GetComponent<MovementFloor>().reset();
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.name != "five") return;

        foreach(GameObject trigger in triggers) {
            if (col.IsTouching(trigger.GetComponent<BoxCollider2D>()) == false)
                continue;
            if (trigger.name == "TubeTrigger") {
                ball2.SetActive(true);
                trigger.SetActive(false);
            }
            else if (trigger.name == "Floor4Trigger") {
                walls["wall7"].GetComponent<MovementFloor>().enableMovement = true;
                walls["wall8"].GetComponent<MovementFloor>().enableMovement = true;
                trigger.SetActive(false);
            }
            else if(trigger.name == "Floor3Trigger") {
                walls["wall9"].GetComponent<MovementFloor>().enableMovement = true;
                walls["wall10"].GetComponent<MovementFloor>().enableMovement = true;
                trigger.SetActive(false);
            }
        }


    }

    private GameObject createElevator() {
        Vector3 elevatorPos = new Vector3(1.51f, -1.99f, 0f);
        GameObject elev = (GameObject)Instantiate(elevatorPrefab, elevatorPos, Quaternion.identity);
        elev.transform.localScale = new Vector3(1.474343f, 1f, 1f);
        MovementFloor script = elev.GetComponent<MovementFloor>();
        script.speed = 3f;
        script.finalPos = new Vector3(1.51f, -0.74f, 0f);
        script.waitingTime = 2;
        script.enableMovement = true;
        elev.GetComponentInChildren<AreaEffector2D>().drag = 0f;
        return elev;
    }



}
