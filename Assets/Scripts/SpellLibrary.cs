using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLibrary : MonoBehaviour {

	public static SpellLibrary instance;

	public ISpell[] spellList;
	Dictionary<string, ISpell> spellDict = new Dictionary<string, ISpell>();

	void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);

			for (int i = 0; i < spellList.Length; i++) {
				spellDict.Add (spellList [i].spellName, spellList [i]);
			}
		}
	}

	public void castSpell(string spellName, PlayerController owner){
		if (spellDict.ContainsKey (spellName)) {
			spellDict [spellName].cast (owner);
		} else {
			print ("Spell not found: " + spellName);
		}
	}
}
