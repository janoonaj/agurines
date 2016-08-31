using UnityEngine;
using System.Collections;

public class ConfigureGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}