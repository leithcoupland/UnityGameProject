using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {

	public SoundGroup[] soundGroups;

	Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    // On startup the method takes all the clips in the sound library object and creates dictionaries for each sound group
	void Awake(){
		foreach (SoundGroup soundGroup in soundGroups){
			groupDictionary.Add (soundGroup.groupID, soundGroup.group);
		}
	}

    // Uses input string as the groupID and randomly selects and returns a clip from the related dictionary
	public AudioClip GetClipFromName(string name){
		if (groupDictionary.ContainsKey (name)) {
			AudioClip[] sounds = groupDictionary [name];
			return sounds[Random.Range (0, sounds.Length)];
		}
		return null;
	}

	[System.Serializable]
	public class SoundGroup{
		public string groupID;
		public AudioClip[] group;
	}
}
