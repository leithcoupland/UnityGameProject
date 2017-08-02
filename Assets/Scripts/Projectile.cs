using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

	public LayerMask collisionMask;
	public float lifetime = 5f;
	public float range = 5;
	public float speed = 10f;
	public float force = 30f;
	public float size = 1;

	PlayerController owner;

	public void setSpeed(float _speed){
		speed = _speed;
	}

	public void setOwner(PlayerController _owner){
		owner = _owner;
	}

	void Start(){
		transform.localScale = transform.localScale * size;
	}

	//[ServerCallback]	// will require movement prediction on client side
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		//CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * Time.deltaTime * speed);

		lifetime -= Time.deltaTime;
		range -= moveDistance;
		if (lifetime <= 0 || range <= 0) {
			NetworkServer.Destroy (gameObject);
		}
	}
	/*
	void CheckCollisions(float moveDistance){
		if (!isServer) {
			return;
		} else {
			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
				OnHitObject (hit);
			}
		}
	}

	void OnHitObject(RaycastHit hit){
		NetworkServer.Destroy (gameObject);
	}*/
		
	void OnTriggerEnter(Collider col){
		if (!isServer) {
			return;
		} 
		if (col.gameObject.tag == "Player" && col.GetComponent<PlayerController>() != owner) {
			PlayerController player = col.GetComponent<PlayerController> ();
			Vector3 heightCorrectedPos = new Vector3 (transform.position.x, col.transform.position.y, transform.position.z);
			player.Push ((col.transform.position - heightCorrectedPos).normalized * force);
			NetworkServer.Destroy (gameObject);
		}
	}
}
