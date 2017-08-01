using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSyncObjectPos : NetworkBehaviour {

	[SerializeField]
	float lerpRate = 5; // lower rate = more smoothing, but potentially greater "lag"

	[SyncVar] 
	private Vector3 syncPos;

	private NetworkIdentity netID;
	private Vector3 lastPos;
	private float threshold = 0.01f;

	void Start(){
		syncPos = transform.position;
	}

	void Update(){
		SendPosition ();
		LerpPosition ();
	}

	void LerpPosition(){
		if (!hasAuthority) {
			transform.position = Vector3.Lerp (transform.position, syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdSendPosition(Vector3 pos){
		syncPos = pos;
	}

	[ClientCallback]
	void SendPosition(){
		if (hasAuthority && Vector3.Distance (transform.position, lastPos) > threshold) {
			CmdSendPosition (transform.position);
			lastPos = transform.position;
		}
	}
}
