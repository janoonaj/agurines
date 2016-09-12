using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
    private Camera camera;
    private Vector3 initialCameraPos;
    private Vector3 lastCameraPos;
    private float CAMERA_WIDTH;
    private float CAMERA_HEIGHT;

    void Start() {
        camera = Camera.main;
        lastCameraPos = initialCameraPos = camera.transform.position;
        CAMERA_HEIGHT = 2f * camera.orthographicSize;
        CAMERA_WIDTH = CAMERA_HEIGHT * camera.aspect;
    }

    void FixedUpdate() {
        Vector3 cameraPos = camera.transform.position;
        if(lastCameraPos != cameraPos) {
            Vector3 offset = (lastCameraPos - initialCameraPos);
            offset = adaptOffsetToCamera(offset);
            GetComponent<Renderer>().material.mainTextureOffset = offset;
            lastCameraPos = cameraPos;
        }
    }

    /*
     * On textures, offset goes between 0 and 1. Adapt offset to camera position.
     */ 
    private Vector3 adaptOffsetToCamera(Vector3 offset) {
        Vector3 result = new Vector3(offset.x, offset.y, offset.z);
        result.x /= CAMERA_WIDTH;
        result.y /= CAMERA_HEIGHT;
        return result;
    }

}