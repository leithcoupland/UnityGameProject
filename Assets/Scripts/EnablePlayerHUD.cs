using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class EnablePlayerHUD : MonoBehaviour
{
    public int playerNum;
	
    //Enable only the number of player information as the number of players
	void Start ()
    {
        if (playerNum > XCI.GetNumPluggedCtrlrs())
        {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
