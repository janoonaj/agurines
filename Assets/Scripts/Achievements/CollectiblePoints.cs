using UnityEngine;
using System.Collections;

/**
 * Points dispersed on the game scene.
 * If player gets all of them, wins the star for this level.
 */ 

public class CollectiblePoints : MonoBehaviour {
    private int recollected = 0;
    private int totalPointsOnScene;

	// Use this for initialization
	void Start () {
        totalPointsOnScene = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }
	
	public void recollectPoint() {
        recollected++;
    }

    public bool recollectedAll() {
        return totalPointsOnScene == recollected;
    }
}
