using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {

	public LayerMask collisionMask;
	public float lifetime = 5f;
	float speed = 10;

	public void setSpeed(float _speed){
		speed = _speed;
	}

	//[ServerCallback]	// tracking movement on server only creates sync problems - either uneven movement, or clients lag behind server when using interpolation.
						// instead, only check for collisions on server.
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * Time.deltaTime * speed);

		lifetime -= Time.deltaTime;
		if (lifetime <= 0) {
			NetworkServer.Destroy (gameObject);
		}
	}

	void CheckCollisions(float moveDistance){
		if (!isServer) {
			return;
		} else {
			Ray ray = new Ray (transform.position, transform.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
				//OnHitObject (hit);
			}
		}
	}

	void OnHitObject(RaycastHit hit){
		
		//NetworkServer.Destroy (gameObject);
	}
}
