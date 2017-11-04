using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoundManager : MonoBehaviour {

	struct PlayerStats{
		public int playerNum;
		public int kills;
		public int deaths;
		public float score;
		public int scoreAsInt(){
			return (int)score;
		}
	}

	public static GameRoundManager instance;

	public GameObject scoreScreen;
	public GameObject[] playerScoreEntries;
	public GameObject roundCountdownText;
	public GameObject roundHeading;

	PlayerStats[] playerStats;
	public int numPlayers = 4;
	public int scorePerWin = 100;
	public int scorePerKill = 100;
	public int scorePerDmg = 1;

	private int numRounds = 3;
	private int roundsPlayed = 0;
	private int numDeadPlayers = 0;

	private bool betweenRounds = false;
	private float timeBetweenRounds = 10;
	private float roundCountdown = 0;


	void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start(){
		if (roundsPlayed == 0) {
			ResetStats ();
		}
	}

	void Update(){
		UpdateScoreScreen ();
		scoreScreen.gameObject.SetActive(Input.GetKey (KeyCode.Tab) || betweenRounds);
		if (betweenRounds) {
			RoundCountdown ();
		}
	}

	void RoundCountdown(){
		roundCountdown += Time.deltaTime;
		if (roundCountdown >= timeBetweenRounds) {
			betweenRounds = false;
			if (roundsPlayed >= numRounds) {
				SceneManager.LoadScene ("Menu");
			} else {
				StartNewRound ();
			}
		}
	}

	void ResetStats(){
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

	void StartNewRound(){
		numDeadPlayers = 0;
		SceneManager.LoadScene ("Main");
	}

	public void SetRoundLimit(int _numRounds){
		numRounds = _numRounds;
	}

	public void PlayerDeath(int deadPlayerNum, int killerPlayerNum){
		numDeadPlayers += 1;
		playerStats [deadPlayerNum-1].deaths += 1;
		if (deadPlayerNum != killerPlayerNum) {
			playerStats [killerPlayerNum - 1].kills += 1;
			playerStats [killerPlayerNum - 1].score += scorePerKill;
		}
		CheckRoundOver (killerPlayerNum);
	}

	void CheckRoundOver(int killerPlayerNum){
		if (numDeadPlayers >= numPlayers - 1) {
			playerStats [killerPlayerNum - 1].score += scorePerWin;
			Invoke ("EndRound", 3);
		}
	}

	void EndRound(){
		betweenRounds = true;
		roundCountdown = 0;
		roundsPlayed += 1;
		SceneManager.LoadScene ("ScoreScreen");
	}

	public void AddScore(int playerNum, float score){
		playerStats [playerNum - 1].score += score * scorePerDmg;
	}

	void UpdateScoreScreen(){
		for (int i = 0; i < 4; i++) {
			if (i < numPlayers){
				PlayerStats ps = playerStats [i];
				playerScoreEntries [i].GetComponent<Text> ().text = "Player " + ps.playerNum + "\t\t\t\t\t\t" + ps.kills + "\t\t\t\t\t\t" + ps.deaths + "\t\t\t\t\t\t" + ps.scoreAsInt();
			} else {
				playerScoreEntries [i].GetComponent<Text> ().text = "";
			}
		}
		if (betweenRounds) {
			roundHeading.GetComponent<Text> ().text = "ROUND " + roundsPlayed + " OF " + numRounds;
			int countdown = (int)(timeBetweenRounds - roundCountdown) + 1;
			string sOrNoS = countdown == 1 ? " second!" : " seconds!";
			if (roundsPlayed < numRounds) {
				roundCountdownText.GetComponent<Text> ().text = "Next round starts in " + countdown + sOrNoS;
			} else {
				roundCountdownText.GetComponent<Text> ().text = "Returning to main menu in " + countdown + sOrNoS;
			}
		} else {
			roundHeading.GetComponent<Text> ().text = "ROUND " + (roundsPlayed + 1) + " OF " + numRounds;
			roundCountdownText.GetComponent<Text> ().text = "";
		}
	}
		
}
