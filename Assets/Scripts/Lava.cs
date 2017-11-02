using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

	public GameObject[] lavaSections;
	public float speed = 1;

	void LateUpdate(){
		Vector3 shift = new Vector3 (0, 0, Time.deltaTime * speed); 
		foreach (GameObject lavaSection in lavaSections) {
			lavaSection.transform.position += shift;
			if (lavaSection.transform.position.z >= 200) {
				lavaSection.transform.position += new Vector3 (0, 0, -400);
			}
		}
	}
}