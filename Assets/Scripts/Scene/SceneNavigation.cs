using UnityEngine;
//using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour {
    static private int currentScene = 0;

	public void changeScene(int level) {

        /*if (Advertisement.IsReady()) {
            Advertisement.Show();
        }*/

    SceneManager.LoadScene(level);
        currentScene = level;
    }

    public void gotoNextLevel() {
        changeScene(currentScene + 1);
    }
}
