using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip mainTheme;
	public AudioClip menuTheme;

    //Play main theme music.
	void Start(){
		AudioManager.instance.PlayMusic (menuTheme, 2);
	}

    //Pause if space pressed.
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			AudioManager.instance.PlayMusic (mainTheme, 3);
		}
	}
}
