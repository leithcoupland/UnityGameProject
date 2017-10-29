using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour {

	MapNavigator mapNavigator;
	Vector3 destination;
	bool dead;
	public int playerNum { get; private set; }

	void Start(){
		mapNavigator = MapNavigator.instance;
		dead = false;
		StartCoroutine (CheckPlatformExpired());
	}

	IEnumerator CheckPlatformExpired(){
		float refreshRate = .2f;
		while (!dead) {
			Platform currentPlatform = mapNavigator.ClosestSegment (transform.position).GetComponent<Platform> ();
			if (currentPlatform != null && currentPlatform.expired) {
				ArrayList reachablePositions = mapNavigator.ReachableSegments (transform.position);
				if (reachablePositions.Count > 0) {
					GameObject destinationPlatform = (GameObject)reachablePositions[Random.Range(0, (int)(reachablePositions.Count - 1))];
					destination = destinationPlatform.transform.position;
				}
			}
			yield return new WaitForSeconds (refreshRate);
		}
	}
}
