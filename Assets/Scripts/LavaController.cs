using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour {

	float speed = 1;

	void LateUpdate(){
		transform.position += new Vector3(0, 0, Time.deltaTime * speed);
		if (transform.position.z >= 200) {
			transform.position += new Vector3(0, 0, -400);
		}
	}
}