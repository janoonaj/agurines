using UnityEngine;
using System.Collections;

public class ConfigureGame : MonoBehaviour {

    void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
    }
}