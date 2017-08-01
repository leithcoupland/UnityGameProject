using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLibrary : MonoBehaviour {

	public static SpellLibrary instance;

	public Spell[] spellList;
	Dictionary<string, Spell> spellDict = new Dictionary<string, Spell>();

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

	public Spell GetSpellFromName(string spellName){
		if (spellDict.ContainsKey (spellName)) {
			return spellDict [spellName];
		} else {
			Debug.Log ("Spell not found: " + spellName);
			return null;
		}
	}

}
