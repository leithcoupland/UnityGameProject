using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	public XboxController xbController;
	public int playerNum { get; private set; }
	PlayerController playerController;

	void Awake(){
		switch (xbController){
			case XboxController.First: playerNum = 1; break;
			case XboxController.Second: playerNum = 2; break;
			case XboxController.Third: playerNum = 3; break;
			case XboxController.Fourth: playerNum = 4; break;
		}
	}

	void Start(){
		playerController = GetComponent<PlayerController> ();
		if (playerNum > GameRoundManager.instance.numPlayers) {
			Destroy (gameObject);
		}
		else if (playerNum > XCI.GetNumPluggedCtrlrs()){
			GetComponent<AIPlayer> ().enabled = true;
			GetComponent<Player> ().enabled = false;
		}
		//Debug.Log(XCI.GetNumPluggedCtrlrs() + " Xbox controllers plugged in.");
	}

	void Update(){
		playerController.Move (new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, xbController), 0, XCI.GetAxis(XboxAxis.LeftStickY, xbController)));
		playerController.Aim (new Vector3(XCI.GetAxis(XboxAxis.RightStickX, xbController), 0, XCI.GetAxis(XboxAxis.RightStickY, xbController)));

		if (XCI.GetButtonDown (XboxButton.RightBumper, xbController)) {
			playerController.Attack ();
		}
	}
}