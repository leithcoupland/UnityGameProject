using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

	[SyncVar]
	public string playerName = "player";

	[SyncVar]
	public Color playerColor = Color.white;

	void OnGUI(){
		if (isLocalPlayer) {
			playerName = GUI.TextField (new Rect (25, Screen.height - 40, 100, 30), playerName);
			if (GUI.Button (new Rect (130, Screen.height - 40, 80, 30), "Change")) {
				CmdChangeName (playerName);
			}
		}
	}

	[Command]
	public void CmdChangeName(string newName){
		playerName = newName;
	}

	void Start() {
		if (isLocalPlayer) {
			GetComponent<Player> ().enabled = true;
		}

		Renderer[] rends = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in rends) {
			r.material.color = playerColor;
		}
		transform.position = new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
	}

	void Update(){
		GetComponentInChildren<TextMesh>().text = playerName;
	}

}
