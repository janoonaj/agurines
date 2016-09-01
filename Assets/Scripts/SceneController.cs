using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void changeScene(int level) {
        SceneManager.LoadScene(level);
    }
}
