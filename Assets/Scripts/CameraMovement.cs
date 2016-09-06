using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    private GameObject player;
    private const float smoothTimeX = 0.05f;
    private const float smoothTimeY = 0.05f;
    private float[] MARGIN_X = { 0.65f, 0.35f };
    private float[] MARGIN_Y = { 0.65f, 0.35f };
    private bool cameraMovementEnable = true;

    public void Start() {
        player = GameObject.Find("five");
    }

    //TODO: soft the camera movement a little bit with smoothdamp
    void FixedUpdate () {
        if (cameraMovementEnable == false) return;

        Vector3 viewPosition = GetComponent<Camera>().WorldToViewportPoint(player.transform.position);

        Vector3 playerMove = player.GetComponent<Five>().currentPos - player.GetComponent<Five>().lastPos;
        Vector3 target = transform.position + playerMove;

        if ((viewPosition.x > MARGIN_X[0] && movingRight(playerMove)) || 
            (viewPosition.x < MARGIN_X[1] && movingLeft(playerMove))) {
            //float posX = Mathf.SmoothDamp(transform.position.x, target.x, ref velocity.x, smoothTimeX);
            float posX = target.x;
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }

        if ((viewPosition.y > MARGIN_Y[0] && movingUp(playerMove))
            || (viewPosition.y < MARGIN_Y[1]) && movingDown(playerMove)) {
            //float posY = Mathf.SmoothDamp(transform.position.y, target.y, ref velocity.y, smoothTimeY);
            float posY = target.y;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }

    private bool movingRight(Vector3 playerMove) {
        return playerMove.x > 0;
    }

    private bool movingLeft(Vector3 playerMove) {
        return playerMove.x < 0;
    }

    private bool movingUp(Vector3 playerMove) {
        return playerMove.y > 0;
    }

    private bool movingDown(Vector3 playerMove) {
        return playerMove.y < 0;
    }

    public IEnumerator centerOn(Vector3 pos) {
        cameraMovementEnable = false;
        transform.position = pos;
        yield return new WaitForSeconds(2);
        cameraMovementEnable = true;
    }
}
