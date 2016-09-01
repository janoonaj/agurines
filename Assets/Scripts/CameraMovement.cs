using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    public GameObject player;
    private const float smoothTimeX = 0.05f;
    private const float smoothTimeY = 0.05f;
    private Vector2 velocity;
    private float[] MARGIN_X = { 0.65f, 0.35f };
    private float[] MARGIN_Y = { 0.55f, 0.45f };

    //TODO: soft the camera movement a little bit with smoothdamp
    void FixedUpdate () {
        Vector3 viewPosition = GetComponent<Camera>().WorldToViewportPoint(player.transform.position);

        Vector3 playerMove = player.GetComponent<Five>().currentPos - player.GetComponent<Five>().lastPos;
        Vector3 target = transform.position + playerMove;

        if (viewPosition.x > MARGIN_X[0] || viewPosition.x < MARGIN_X[1]) {
            //float posX = Mathf.SmoothDamp(transform.position.x, target.x, ref velocity.x, smoothTimeX);
            float posX = target.x;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }

        if (viewPosition.y > MARGIN_Y[0] || viewPosition.y < MARGIN_Y[1]) {
            //float posY = Mathf.SmoothDamp(transform.position.y, target.y, ref velocity.y, smoothTimeY);
            float posY = target.y;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }
}
