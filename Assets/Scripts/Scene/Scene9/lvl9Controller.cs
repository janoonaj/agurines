using UnityEngine;
using System.Collections;
using System;

public class lvl9Controller : MonoBehaviour, SceneController {
    public GameObject ballPrefab;
    private GameObject ball1;
    private GameObject ball2;


    void Start () {
        //UnityEngine.Object sball1 = 

        ball1 = (GameObject) Instantiate(ballPrefab, new Vector3(0.39f, 3.44f, 0f), Quaternion.identity);


	}

	void Update () {
	
	}

    public void buttonPush(GameObject button) {
        if(button.name == "button1") {
            ball1.GetComponent<Five>().freeze();
        } 
        else if (button.name == "button2") {
            //button.SetActive(false);
        }
    }
}
