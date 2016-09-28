using UnityEngine;
using System.Collections;

public class ShakeMobile : MonoBehaviour {

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
    float shakeDetectionThreshold = 2.0f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private Vector3 acceleration;
    private Vector3 deltaAcceleration;

    //Just one shake each second
    private float currentCoolDownTime = 0f;
    private const float COOL_DOWN_TIME = 1f;
    private bool coolingDown = false;

    private const float SHAKE_FORCE = 4f;


    // Use this for initialization
    void Start () {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }
	
	// Update is called once per frame
	void Update () {
        coolingDownTime();
        if (isShackingDevice() && coolingDown == false) {
            shake();
        }
    }

    private void coolingDownTime() {
        if (coolingDown == false) return;
      
        currentCoolDownTime += Time.deltaTime;
        if (currentCoolDownTime >= COOL_DOWN_TIME)
            coolingDown = false;
    }

    private bool isShackingDevice() {
        acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        deltaAcceleration = acceleration - lowPassValue;
        return deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold;
    }

    private void shake() {
        coolingDown = true;
        currentCoolDownTime = 0f;
        GameObject.Find("five").GetComponent<Rigidbody2D>().AddForce(new Vector2(0, SHAKE_FORCE), ForceMode2D.Impulse);
    }

    public void printME() {
        print("TOCADO");
    }

}
