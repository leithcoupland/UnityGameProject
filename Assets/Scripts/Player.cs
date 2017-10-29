using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	public XboxController controller;
	public int playerNum { get; private set; }
	PlayerController playerController;

	void Start(){
		playerController = GetComponent<PlayerController> ();

		switch (controller){
		case XboxController.First: playerNum = 1; break;
		case XboxController.Second: playerNum = 2; break;
		case XboxController.Third: playerNum = 3; break;
		case XboxController.Fourth: playerNum = 4; break;
		}
		if (playerNum > XCI.GetNumPluggedCtrlrs()){
			Destroy(gameObject);
		}
		Debug.Log(XCI.GetNumPluggedCtrlrs() + " Xbox controllers plugged in.");
	}

	void Update(){
		playerController.Move (new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, controller), 0, XCI.GetAxis(XboxAxis.LeftStickY, controller)));
		playerController.Aim (new Vector3(XCI.GetAxis(XboxAxis.RightStickX, controller), 0, XCI.GetAxis(XboxAxis.RightStickY, controller)));

		if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
			playerController.Attack ();
	}
}