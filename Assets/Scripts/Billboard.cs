using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	
	/*void Update () {
		transform.LookAt (Camera.main.transform.position);
		transform.Rotate (new Vector3 (0, 180, 0));
	}*/

	void LateUpdate () {
		transform.LookAt (Camera.main.transform.position + (Camera.main.transform.forward * int.MaxValue));
	}

}
