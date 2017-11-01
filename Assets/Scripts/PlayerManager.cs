using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager instance;

	public int numPlayers = 4;

	public PlayerController player1;
	public PlayerController player2;
	public PlayerController player3;
	public PlayerController player4;

	void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
}
