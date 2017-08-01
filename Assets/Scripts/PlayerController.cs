using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : NetworkBehaviour {

	public Projectile projectile;

	public Transform spellOrigin;
	public string[] spellNames;
	Spell[] spells;

	Vector3 velocity;
	Rigidbody myRigidbody;
	SpellLibrary spellLibrary;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
		spellLibrary = SpellLibrary.instance;
		spells = new Spell[spellNames.Length];
		for (int i = 0; i < spellNames.Length; i++) {
			spells [i] = spellLibrary.GetSpellFromName (spellNames [i]);
		}
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

	[Command]
	public void CmdCastSpell(int spellIndex){
		GameObject fireball = Instantiate(projectile.gameObject, spellOrigin.position, spellOrigin.rotation) as GameObject;
		NetworkServer.Spawn(fireball);
	}

}
