using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigator : MonoBehaviour {

	public GameObject[] mapSegments;
	public float segmentRadius;
	public float radiusThreshold;
	private Dictionary<GameObject, ArrayList> navGraph;

	public static MapNavigator instance;

	public GameObject ClosestSegment(Vector3 position){
		GameObject closestSeg = null;
		float minSqrDist = float.MaxValue;
		for (int i = 0; i < mapSegments.Length; i++) {
			float sqrDist = (position - mapSegments [i].transform.position).sqrMagnitude;
			if (sqrDist <= minSqrDist) {
				minSqrDist = sqrDist;
				closestSeg = mapSegments [i];
			}
		}
		return closestSeg;
	}

	public ArrayList ReachableSegments(Vector3 position){
		return navGraph[ClosestSegment(position)];
	}

	void Awake(){
		instance = this;
	}

	void Start(){
		navGraph = new Dictionary<GameObject, ArrayList> ();
		StartCoroutine (MaintainNavGraph ());
	}

	private void UpdateNavGraph(){
		navGraph.Clear ();
		for (int i = 0; i < mapSegments.Length; i++) {
			for (int j = 0; j < mapSegments.Length; j++) {
				if (Connected (mapSegments [i], mapSegments [j]) && !ExpiredSegment (mapSegments [j])) {
					if (navGraph.ContainsKey (mapSegments [i])) {
						navGraph [mapSegments [i]].Add (mapSegments [j]);
					} else {
						ArrayList segList = new ArrayList ();
						segList.Add (mapSegments [j]);
						navGraph.Add (mapSegments [i], segList);
					}
				}
			}
		}
	}

	private bool Connected(GameObject segA, GameObject segB){
		Vector3 posA = segA.transform.position;
		Vector3 posB = segB.transform.position;
		float sqrConnectedDist = (segmentRadius * 2 + radiusThreshold) * (segmentRadius * 2 + radiusThreshold);
		return ((posA - posB).sqrMagnitude <= sqrConnectedDist);
	}

	private bool ExpiredSegment(GameObject segment){
		if (segment.GetComponent<Platform> () != null) {
			Platform p = segment.GetComponent<Platform> ();
			if (p.expired) {
				return true;
			}
		}
		return false;
	}

	IEnumerator MaintainNavGraph(){
		float refreshRate = .5f;
		UpdateNavGraph ();
		yield return new WaitForSeconds (refreshRate);
	}
}
