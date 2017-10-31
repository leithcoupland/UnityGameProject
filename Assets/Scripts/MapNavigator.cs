using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigator : MonoBehaviour {

	public GameObject platformHolder;
	public float platformRadius;
	public float radiusThreshold;
	private Platform[] platforms;
	private Dictionary<Platform, ArrayList> navGraph;

	public static MapNavigator instance;

	void Awake(){
		instance = this;
		PopulatePlatformArray ();
		navGraph = new Dictionary<Platform, ArrayList> ();
		StartCoroutine (MaintainNavGraph ());
	}

	void Start(){
		
	}

	private void PopulatePlatformArray(){
		Platform[] p1Platforms = platformHolder.transform.GetChild (0).GetComponentsInChildren<Platform> ();
		Platform[] p2Platforms = platformHolder.transform.GetChild (1).GetComponentsInChildren<Platform> ();
		Platform[] p3Platforms = platformHolder.transform.GetChild (2).GetComponentsInChildren<Platform> ();
		Platform[] p4Platforms = platformHolder.transform.GetChild (3).GetComponentsInChildren<Platform> ();

		platforms = new Platform[p1Platforms.Length + p2Platforms.Length + p3Platforms.Length + p4Platforms.Length];

		p1Platforms.CopyTo (platforms, 0);
		p2Platforms.CopyTo (platforms, p1Platforms.Length);
		p3Platforms.CopyTo (platforms, p1Platforms.Length + p2Platforms.Length);
		p4Platforms.CopyTo (platforms, p1Platforms.Length + p2Platforms.Length + p3Platforms.Length);
	}

	private void UpdateNavGraph(){
		navGraph.Clear ();
		for (int i = 0; i < platforms.Length; i++) {
			for (int j = 0; j < platforms.Length; j++) {
				if (Connected (platforms [i], platforms [j]) /*&& !platforms [j].expired*/) {
					if (navGraph.ContainsKey (platforms [i])) {
						navGraph [platforms [i]].Add (platforms [j]);
					} else {
						ArrayList platList = new ArrayList ();
						platList.Add (platforms [j]);
						navGraph.Add (platforms [i], platList);
					}
				}
			}
		}
	}

	public Platform ClosestPlatform(Vector3 position){
		Platform closestPlatform = null;
		float minSqrDist = float.MaxValue;
		for (int i = 0; i < platforms.Length; i++) {
			float sqrDist = (position - platforms [i].transform.position).sqrMagnitude;
			if (sqrDist <= minSqrDist) {
				minSqrDist = sqrDist;
				closestPlatform = platforms [i];
			}
		}
		return closestPlatform;
	}

	public Platform ClosestSafePlatform(Vector3 position){
		Platform closestPlatform = null;
		float minSqrDist = float.MaxValue;
		for (int i = 0; i < platforms.Length; i++) {
			float sqrDist = (position - platforms [i].transform.position).sqrMagnitude;
			if (!platforms[i].expired && sqrDist <= minSqrDist) {
				minSqrDist = sqrDist;
				closestPlatform = platforms [i];
			}
		}
		return closestPlatform;
	}

	public Platform RandomSafePlatform(){
		ArrayList safePlatforms = new ArrayList ();
		foreach (Platform platform in platforms) {
			if (!platform.expired) {
				safePlatforms.Add (platform);
			}
		}
		return (Platform)safePlatforms[Random.Range(0, safePlatforms.Count)];
	}

	public ArrayList ConnectedPlatforms(Vector3 position){
		return navGraph[ClosestPlatform(position)];
	}

	public ArrayList ConnectedSafePlatforms(Vector3 position){
		ArrayList allConnectedPlatforms = ConnectedPlatforms (position);
		ArrayList safeConnectedPlatforms = new ArrayList ();

		foreach (Platform platform in allConnectedPlatforms) {
			if (!platform.expired) {
				safeConnectedPlatforms.Add (platform);
			}
		}
		return safeConnectedPlatforms;
	}

	private bool Connected(Platform platA, Platform platB){
		Vector3 posA = platA.transform.position;
		Vector3 posB = platB.transform.position;
		float sqrConnectedDist = (platformRadius * 2 + radiusThreshold) * (platformRadius * 2 + radiusThreshold);
		return ((posA - posB).sqrMagnitude <= sqrConnectedDist);
	}

	public float CrossPlatformSqrDist(){
		return (2 * platformRadius) * (2 * platformRadius);
	}

	public Vector3 RandomPositionOnPlatform(Vector3 plat){
		Vector3 platform = ClosestSafePlatform (plat).transform.position;
		float variance = platformRadius * 0.9f;
		Vector3 randomPos = new Vector3 (platform.x + Random.Range(-variance, variance), platform.y, platform.z + Random.Range(-variance, variance));
		return randomPos;
	}

	IEnumerator MaintainNavGraph(){
		float refreshRate = 1f;
		UpdateNavGraph ();
		yield return new WaitForSeconds (refreshRate);
	}
}
