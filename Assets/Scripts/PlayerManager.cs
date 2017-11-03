using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	struct PlayerStats{
		public int playerNum;
		public int kills;
		public int deaths;
		public int score;
	}

	public static PlayerManager instance;

	public GameObject scoreScreen;
	public GameObject[] playerScoreEntries;

	PlayerStats[] playerStats;
	public int numPlayers = 4;
	public int scorePerKill = 100;

	void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start(){
		playerStats = new PlayerStats[numPlayers];
		for (int i = 0; i < numPlayers; i++) {
			PlayerStats ps;
			ps.playerNum = i + 1;
			ps.kills = 0;
			ps.deaths = 0;
			ps.score = 0;
			playerStats[i] = ps;
		}
	}

	void Update(){
		UpdateScoreScreen ();
		scoreScreen.gameObject.SetActive(Input.GetKey (KeyCode.Tab));
	}

	public void PlayerDeath(int deadPlayerNum, int killerPlayerNum){
		playerStats [deadPlayerNum-1].deaths += 1;
		if (deadPlayerNum != killerPlayerNum) {
			playerStats [killerPlayerNum - 1].kills += 1;
			playerStats [killerPlayerNum - 1].score += scorePerKill;
		}
	}

	void UpdateScoreScreen(){
		for (int i = 0; i < 4; i++) {
			if (i < numPlayers){
				PlayerStats ps = playerStats [i];
				playerScoreEntries [i].GetComponent<Text> ().text = "Player " + ps.playerNum + "\t\t\t\t\t\t" + ps.kills + "\t\t\t\t\t\t" + ps.deaths + "\t\t\t\t\t\t" + ps.score;
			} else {
				playerScoreEntries [i].GetComponent<Text> ().text = "";
			}
		}
	}
}
