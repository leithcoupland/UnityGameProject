using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject healthBar;
	public GameObject staminaBar;
	public GameObject firingPoint;
	public Projectile projectile;

	Rigidbody rigidBody;
	Animator animator;
	public int playerNum { get; private set; }

	public float maxStamina;
	public float stamina { get; private set; }
	public float attackCost;
	public float stamRechargeDelay = 1;
	private float stamRechargeTimer = 0;

	public float movingTurnSpeed = 360;
	public float stationaryTurnSpeed = 180;
	public float moveSpeedMultiplier = 1f;
	public float fireRate;
	private float lastShot = -10.0f;

	private bool isAiming;
	private bool isMoving;
	private float stepDelay = 0.4f;
	private float stepTimer = 0f;

	Vector3 inputVelocity;
	Vector3 pushVelocity;
	float friction = .05f;
	float squaredFrictionThreshold = .01f;

	float hp = 100;
	float maxHp = 100;

	void Start(){
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		if (GetComponent<Player>() != null) {
			playerNum = GetComponent<Player> ().playerNum;
		} else if (GetComponent<AIPlayer>() != null) {
			playerNum = GetComponent<AIPlayer> ().playerNum;
		}
		stamina = maxStamina;
		isMoving = false;
	}

	void Update(){
		CheckMovementStatus ();
		UpdateStamina ();
		UpdateHealthAndStamBars ();
	}

	void FixedUpdate(){
		Vector3 velocity = (inputVelocity + pushVelocity) * Time.fixedDeltaTime;
		rigidBody.MovePosition (rigidBody.position + velocity);
		pushVelocity = pushVelocity * (1 / (friction+1));
		if (pushVelocity.sqrMagnitude <= squaredFrictionThreshold) {
			pushVelocity = Vector3.zero;
		}
	}

	void UpdateHealthAndStamBars(){
		//healthBar.transform.localScale = new Vector3(rigidBody.mass, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
		healthBar.transform.localScale = new Vector3(hp/maxHp, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
		staminaBar.transform.localScale = new Vector3(stamina/maxStamina, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
	}

	void CheckForDeath(){
		if (hp <= 0) {
			Destroy(gameObject);
			AudioManager.instance.PlaySound("death", transform.position);
		}
		/*if (rigidBody.mass <= 0.01){
			Destroy(gameObject);
			AudioManager.instance.PlaySound("death", transform.position);
		}*/
	}

	void UpdateStamina(){
		stamRechargeTimer += Time.deltaTime;
		if (stamina < maxStamina && stamRechargeTimer > stamRechargeDelay){
			stamina += Time.deltaTime/4;
			if (stamina > maxStamina) {
				stamina = maxStamina;
			}
		}
	}

	void CheckMovementStatus(){
		if (isMoving) {
			if (stepTimer < stepDelay){
				stepTimer += Time.deltaTime;
			} else {
				stepTimer = 0;
				AudioManager.instance.PlaySound("footsteps", transform.position);
			}
		}
		animator.SetBool("isRunning", isMoving);
		animator.SetBool ("isIdle", !isMoving);
	}

	public void Attack(){
		if (Time.time > (fireRate + lastShot) && stamina > 0)
		{
			if (stamina < attackCost){
				stamina = 0;
				stamRechargeTimer = 0;
			} else {
				stamina -= attackCost;
				stamRechargeTimer = 0;
			}

			animator.SetBool("isCasting", true);
			Projectile p = Instantiate(projectile, firingPoint.transform.position, transform.rotation);
			p.setPlayerNo (playerNum);
			AudioManager.instance.PlaySound("spell", p.transform.position);
			lastShot = Time.time;
		}
	}

	public void Move(Vector3 _inputVelocity){
		if (_inputVelocity.magnitude > 1f) {
			_inputVelocity = _inputVelocity.normalized;
		}
		if (isMoving = _inputVelocity.magnitude > 0){
			animator.SetBool("isCasting", false);
		}
		inputVelocity = _inputVelocity * moveSpeedMultiplier;
		if (!isAiming) {
			FaceDirection (_inputVelocity);
		}
	}

	public void Aim(Vector3 aimInput){
		isAiming = aimInput.magnitude > 0;
		if (!isAiming) {
			return;
		}
		Vector3 aim = aimInput;
		if (aim.magnitude > 1f) {
			aim.Normalize ();
		}
		animator.SetBool("isCasting", false);
		FaceDirection (aim);
	}

	public void Push (Vector3 _pushVelocity){
		float missingHPCoefficient = 1 - (hp/maxHp); // 0 for full hp, 1 for no hp.
		float pushAdjustment = 2 + 4 * (missingHPCoefficient);
		pushVelocity += _pushVelocity * pushAdjustment;
		AudioManager.instance.PlaySound ("grunt", transform.position);
	}

	public void Damage(float damage){
		hp -= damage;
		CheckForDeath ();
	}

	void FaceDirection(Vector3 direction){
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("CastPrimary") || animator.GetCurrentAnimatorStateInfo(0).IsName("CastAOE")){
			return;
		}
		Vector3 dir = direction;
		dir = transform.InverseTransformDirection(dir);
		dir = Vector3.ProjectOnPlane(dir, new Vector3(0,0,0));
		float turnAmount = Mathf.Atan2(dir.x, dir.z);
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, dir.z);
		transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
	}
}