using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISpell : MonoBehaviour {

	public string spellName;

	public virtual void cast (PlayerController owner){
		// override for each spell
	}

}
