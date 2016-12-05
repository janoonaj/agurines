using UnityEngine;
using System.Collections;
using System;

public class Lvl11Controller : MonoBehaviour, SceneController {
    private GameObject button3;

    // Use this for initialization
    void Start () {
        button3 = GameObject.Find("button (3)");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void buttonPush(GameObject button) {
        if (button.name == "button (3)") {
            button3.SetActive(false);
        }
    }

    public void reset() {
        //throw new NotImplementedException();
    }
}
