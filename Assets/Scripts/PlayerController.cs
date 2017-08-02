using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : NetworkBehaviour {

	public Projectile projectile;
	public Transform spellOrigin;

	Vector3 inputVelocity;
	[SyncVar]
	Vector3 pushVelocity;

	float friction = .05f;
	float squaredFrictionThreshold = .01f;

	Rigidbody myRigidbody;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 _inputVelocity){
		inputVelocity = _inputVelocity;
	}

	public void Push(Vector3 _pushVelocity){
		pushVelocity += _pushVelocity;
	}

	public void LookAt(Vector3 lookPoint){
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

	public void FixedUpdate(){
		Vector3 velocity = inputVelocity + pushVelocity;
		myRigidbody.MovePosition (myRigidbody.position + velocity * Time.fixedDeltaTime);
		pushVelocity = pushVelocity * (1 / (Time.fixedDeltaTime * 50 * (friction+1)));
		if (pushVelocity.sqrMagnitude <= squaredFrictionThreshold) {
			pushVelocity = Vector3.zero;
		}
	}

	[Command]
	public void CmdCastSpell(int spellIndex){
		Projectile fireball = Instantiate(projectile, spellOrigin.position, spellOrigin.rotation) as Projectile;
		fireball.setOwner (this);
		NetworkServer.Spawn(fireball.gameObject);
	}

}
