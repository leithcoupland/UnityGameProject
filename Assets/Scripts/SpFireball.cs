using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpFireball : Spell{

	public Projectile projectile;

	void Start () {
		spellName = "Fireball";
	}
		
	public override void Cast (PlayerController owner){
		GameObject fireball = Instantiate(projectile.gameObject, owner.spellOrigin.position, owner.spellOrigin.rotation) as GameObject;
		CmdSpawn (fireball);
	}

	[Command]
	void CmdSpawn(GameObject obj){
		NetworkServer.Spawn(obj);
		//NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
	}

}
