using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	public Transform spellOrigin;
	public string[] spells;

	Vector3 velocity;
	Rigidbody myRigidbody;
	SpellLibrary spellLibrary;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
		spellLibrary = SpellLibrary.instance;
	}

	public void Move(Vector3 _velocity){
		velocity = _velocity;
	}

	public void LookAt(Vector3 lookPoint){
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

	public void FixedUpdate(){
		myRigidbody.MovePosition (myRigidbody.position + velocity * Time.fixedDeltaTime);
	}

	public void castSpell(int spellIndex){
		if (spellIndex < spells.Length && spells [spellIndex] != null) {
			spellLibrary.castSpell (spells [spellIndex], GetComponent<PlayerController> ());
		}
	}
}
