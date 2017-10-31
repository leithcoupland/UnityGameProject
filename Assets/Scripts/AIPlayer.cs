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
		StartCoroutine (MoveToSafePlatform());
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

	private Vector3 RandomPositionOnPlatform(Vector3 platform){
		float variance = mapNavigator.platformRadius * 0.9f;
		Vector3 randomPos = new Vector3 (platform.x + Random.Range(-variance, variance), platform.y, platform.z + Random.Range(-variance, variance));
		return randomPos;
	}

	IEnumerator MoveToSafePlatform(){
		float refreshRate = .2f;
		while (!dead) {
			if (mapNavigator.ClosestPlatform (transform.position) != null) {
				Platform currentPlatform = mapNavigator.ClosestPlatform (transform.position);
				if (currentPlatform.pastExpired || SqrDistToNearestSafePlatform() > mapNavigator.CrossPlatformSqrDist()) {
					destination = RandomPositionOnPlatform(mapNavigator.ClosestSafePlatform (transform.position).transform.position);
				} 
				else if (currentPlatform.expired && mapNavigator.ClosestPlatform(destination).expired) {
					ArrayList reachablePlatforms = mapNavigator.ReachableSafePlatforms (transform.position);
					if (reachablePlatforms.Count > 0) {
						Platform destinationPlatform = (Platform)reachablePlatforms [Random.Range (0, (int)(reachablePlatforms.Count - 1))];
						destination = RandomPositionOnPlatform (destinationPlatform.transform.position);//destinationPlatform.transform.position;
					} else {
						destination = RandomPositionOnPlatform (mapNavigator.ClosestSafePlatform(transform.position).transform.position);//mapNavigator.ClosestSafePlatform (transform.position).transform.position;
					}
				}
			}
			yield return new WaitForSeconds (refreshRate);
		}
	}
}
