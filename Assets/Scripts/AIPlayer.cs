using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class AIPlayer : MonoBehaviour {

	PlayerController playerController;
	MapNavigator mapNavigator;
	Vector3 destination;
	bool dead;
	public int playerNum { get; private set; }

	void Start(){
		playerController = GetComponent<PlayerController> ();
		mapNavigator = MapNavigator.instance;
		dead = false;
		destination = transform.position;
		StartCoroutine (CheckPlatformExpired());
	}

	void Update(){
		moveToDestination ();
	}

	private void moveToDestination(){
		Vector3 direction = destination - transform.position;

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
