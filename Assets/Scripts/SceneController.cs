using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void changeScene(int level) {
        if(level == 0) {
            SceneManager.LoadScene("SelectScreen");
        } else 
            SceneManager.LoadScene("level" + level);
    }
}
