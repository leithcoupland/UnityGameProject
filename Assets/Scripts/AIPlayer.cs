using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class AIPlayer : MonoBehaviour {

	PlayerController playerController;
	MapNavigator mapNavigator;
	Vector3 destination;
	bool dead;

	void Start(){
		playerController = GetComponent<PlayerController> ();
		mapNavigator = MapNavigator.instance;
		dead = false;
		destination = transform.position;
		StartCoroutine (MoveToSafePlatform());
		StartCoroutine (RandomMovement ());
	}

	void Update(){
		MoveToDestination ();
	}

	private void MoveToDestination(){
		destination = new Vector3 (destination.x, transform.position.y, destination.z);
		Debug.DrawLine(transform.position, destination, Color.green);
		Vector3 direction = destination - transform.position;
		playerController.Move (direction);
	}

	private float SqrDistToNearestSafePlatform(){
		return (transform.position - mapNavigator.ClosestSafePlatform (transform.position).transform.position).sqrMagnitude;
	}

	private void FindNearbySafePlatform(){
		ArrayList reachablePlatforms = mapNavigator.ConnectedSafePlatforms (transform.position);
		if (reachablePlatforms.Count > 0) {
			Platform destinationPlatform = (Platform)reachablePlatforms [Random.Range (0, (int)(reachablePlatforms.Count - 1))];
			destination = mapNavigator.RandomPositionOnPlatform (destinationPlatform.transform.position);
		} else {
			destination = mapNavigator.RandomPositionOnPlatform(transform.position);
		}
	}

	IEnumerator MoveToSafePlatform(){
		float refreshRate = .2f;
		while (!dead) {
			Platform currentPlatform = mapNavigator.ClosestPlatform (transform.position);
			if (currentPlatform.pastExpired || SqrDistToNearestSafePlatform() > 1.5 * mapNavigator.CrossPlatformSqrDist()) {
				destination = mapNavigator.RandomPositionOnPlatform(transform.position);
			} 
			else if (currentPlatform.expired && mapNavigator.ClosestPlatform(destination).expired) {
				FindNearbySafePlatform ();
			}
			yield return new WaitForSeconds (refreshRate);
		}
	}

	IEnumerator RandomMovement(){
		float refreshRate = 0.1f;
		Platform oldPlat = mapNavigator.ClosestPlatform(transform.position);
		while (!dead) {
			refreshRate = Random.Range (0.2f, 0.6f);
			if (!mapNavigator.ClosestPlatform (transform.position).expired) {
				if (mapNavigator.ClosestPlatform (transform.position).transform.position == oldPlat.transform.position) {
					destination = mapNavigator.RandomPositionOnPlatform (mapNavigator.RandomSafePlatform ().transform.position);
				}
			}
			oldPlat = mapNavigator.ClosestPlatform(transform.position);
			yield return new WaitForSeconds (refreshRate);
		}

	}
}
