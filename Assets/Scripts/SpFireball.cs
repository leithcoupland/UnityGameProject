using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpFireball : ISpell {

	public Projectile projectile;

	void Start () {
		spellName = "Fireball";
	}
	
	public override void cast (PlayerController owner){
		Instantiate(projectile, owner.spellOrigin.position, owner.spellOrigin.rotation);

	}
}
