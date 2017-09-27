using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hazard : NetworkBehaviour {

	public float damageRate = 20;

	public void ApplyEffect(PlayerController player){
		player.Damage (damageRate * Time.fixedDeltaTime);
	}
}
