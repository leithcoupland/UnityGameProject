using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour {

	MapNavigator mapNavigator;

	Vector3 destination;

	void Start(){
		mapNavigator = MapNavigator.instance;
		StartCoroutine (CheckPlatformExpired());
	}

	IEnumerator CheckPlatformExpired(){
		float refreshRate = .2f;

		yield return new WaitForSeconds (refreshRate);
	}
}
