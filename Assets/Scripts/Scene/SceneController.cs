using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    static private int currentScene = 0;

	public void changeScene(int level) {
        SceneManager.LoadScene(level);
        currentScene = level;
    }

    public void gotoNextLevel() {
        changeScene(currentScene + 1);
    }
}
