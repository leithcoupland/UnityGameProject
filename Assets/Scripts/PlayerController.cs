using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : NetworkBehaviour {

	public Projectile projectile;
	public Transform spellOrigin;
	public LayerMask collisionMask;

	Rigidbody myRigidbody;
	new BoxCollider collider;
	RaycastOrigins raycastOrigins;

	Vector3 inputVelocity;
	Vector3 pushVelocity;
	Vector3 gravVelocity;

	const float raycastOffset = .1f;

	[SyncVar]
	float hp = 100;
	float friction = .05f;
	float squaredFrictionThreshold = .01f;
	float gravity = -20;

	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<BoxCollider> ();
	}

	void UpdateRaycastOrigins(){
		Bounds bounds = collider.bounds;
		bounds.Expand (raycastOffset * -2);

		Vector3 bottomCentre = new Vector3 (transform.position.x, bounds.min.y, transform.position.z);
		float halfSideLength = collider.size.x / 2f;

		raycastOrigins.bottomNorth = bottomCentre + Vector3.forward * halfSideLength;
		raycastOrigins.bottomSouth = bottomCentre - Vector3.forward * halfSideLength;
		raycastOrigins.bottomWest = bottomCentre - Vector3.right * halfSideLength;
		raycastOrigins.bottomEast = bottomCentre + Vector3.right * halfSideLength;
	}

	struct RaycastOrigins{
		public Vector3 bottomNorth, bottomSouth;
		public Vector3 bottomWest, bottomEast;
	}

	public void Move(Vector3 _inputVelocity){
		inputVelocity = _inputVelocity;
	}

	public void Push(Vector3 _pushVelocity){
		pushVelocity += _pushVelocity;
	}

	public void Damage(float dmg){
		hp -= dmg;
		if (hp <= 0) {
			hp = 0;
			NetworkServer.Destroy (gameObject);
		}
	}

	public void LookAt(Vector3 lookPoint){
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

	void FixedUpdate(){
		gravVelocity.y += gravity * Time.fixedDeltaTime;
		Vector3 velocity = (inputVelocity + pushVelocity) * Time.fixedDeltaTime + gravVelocity;

		VerticalCollisions (ref velocity);
		myRigidbody.MovePosition (myRigidbody.position + velocity);

		pushVelocity = pushVelocity * (1 / (friction+1));
		if (pushVelocity.sqrMagnitude <= squaredFrictionThreshold) {
			pushVelocity = Vector3.zero;
		}
	}

	void VerticalCollisions(ref Vector3 velocity){
		float rayLength = Mathf.Abs (velocity.y) + raycastOffset;
		UpdateRaycastOrigins ();

		RaycastHit hit;
		if (Physics.Raycast (raycastOrigins.bottomNorth, Vector2.up * -1, out hit, rayLength, collisionMask)) {
			velocity.y = (hit.distance - raycastOffset) * -1;
			gravVelocity = Vector3.zero;
			rayLength = hit.distance;

			if (hit.transform.gameObject.GetComponent<Hazard> () != null) {
				hit.transform.gameObject.GetComponent<Hazard> ().ApplyEffect (this);
			}
		}

		Debug.DrawRay (raycastOrigins.bottomNorth, Vector3.up * -2, Color.red);
		Debug.DrawRay (raycastOrigins.bottomSouth, Vector3.up * -2, Color.red);
		Debug.DrawRay (raycastOrigins.bottomWest, Vector3.up * -2, Color.red);
		Debug.DrawRay (raycastOrigins.bottomEast, Vector3.up * -2, Color.red);
	}

	[Command]
	public void CmdCastSpell(int spellIndex){
		Projectile fireball = Instantiate(projectile, spellOrigin.position, spellOrigin.rotation) as Projectile;
		fireball.setOwner (this);
		NetworkServer.Spawn(fireball.gameObject);
	}

}
