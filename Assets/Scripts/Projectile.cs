using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	float speed = 10;

	public void setSpeed(float _speed){
		speed = _speed;
	}

	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
}
