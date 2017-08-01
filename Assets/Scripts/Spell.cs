using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spell : NetworkBehaviour {

	public string spellName;

	public virtual void Cast (PlayerController owner){
		// override for each spell
	}

}
