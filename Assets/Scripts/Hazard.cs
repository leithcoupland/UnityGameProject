using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	public float damageRate = 20;

	public void ApplyEffect(PlayerController player){
		player.Damage (damageRate * Time.fixedDeltaTime);
	}
}
