using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Platform : NetworkBehaviour {

	[SyncVar]
	public float timer;
	public bool permanent;

	void Update () {
		if (!isServer || permanent) {
			return;
		}
			
		timer -= Time.deltaTime;

		if (timer <= 0) {
			Activate ();
		}
		else if (timer <= 3){
			Warn();
		}
	}

	void Activate(){
		NetworkServer.Destroy (gameObject);
	}

	void Warn(){
		GetComponent<MeshRenderer> ().material.color = new Color (1.0f, 0.0f, 0.0f, 0.5f);
		/*
		print ("warning");
		Component[] renderers = GetComponents<Renderer> ();
		foreach (Renderer r in renderers) {
			Color color;
			foreach (Material m in r.materials) {
				color = m.color;
				color.a = 0.2f;
				m.color = color;
			}
		}*/
		//Color color = renderer.material.color;
		//color.a = 0.5f;
		//GetComponent<MeshRenderer> ().material.color = color;
	}
}
