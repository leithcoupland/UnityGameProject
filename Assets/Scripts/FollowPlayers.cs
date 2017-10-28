using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayers : MonoBehaviour {


    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    // Use this for initialization
    void Start () {
		
	}
	
    //Move camera position to midpoint between x max of largest x position player and x min of the smallest c position player.
    //Do the same for the y positions.
    //Only calculate on players that exist.
	// Update is called once per frame
	void Update () {
        float xMin = 0;
        float xMax = 0;
        float zMin = 0;
        float zMax = 0;
        float div = 0;

        if (Player1 != null)
        {
            if (xMax < Player1.transform.position.x)
            {
                xMax = Player1.transform.position.x;  
            }
            if (xMin > Player1.transform.position.x)
            {
                xMin = Player1.transform.position.x;
            }

            if (zMax < Player1.transform.position.z)
            {
                zMax = Player1.transform.position.z;
            }
            if (zMin > Player1.transform.position.z)
            {
                zMin = Player1.transform.position.z;
            }
            
            div += 1.0f;
        }

        if (Player2 != null)
        {
            if (xMax < Player2.transform.position.x)
            {
                xMax = Player2.transform.position.x;
            }
            if (xMin > Player2.transform.position.x)
            {
                xMin = Player2.transform.position.x;
            }

            if (zMax < Player2.transform.position.z)
            {
                zMax = Player2.transform.position.z;
            }
            if (zMin > Player2.transform.position.z)
            {
                zMin = Player2.transform.position.z;
            }
            div += 1.0f;
        }


        if (Player3 != null)
        {
            if (xMax < Player3.transform.position.x)
            {
                xMax = Player3.transform.position.x;
            }
            if (xMin > Player3.transform.position.x)
            {
                xMin = Player3.transform.position.x;
            }

            if (zMax < Player3.transform.position.z)
            {
                zMax = Player3.transform.position.z;
            }
            if (zMin > Player3.transform.position.z)
            {
                zMin = Player3.transform.position.z;
            }
            if (div!=2.0f)
            {
                div += 1.0f;
            }
            
        }

        if (Player4 != null)
        {
            if (xMax < Player4.transform.position.x)
            {
                xMax = Player4.transform.position.x;
            }
            if (xMin > Player4.transform.position.x)
            {
                xMin = Player4.transform.position.x;
            }

            if (zMax < Player4.transform.position.z)
            {
                zMax = Player4.transform.position.z;
            }
            if (zMin > Player4.transform.position.z)
            {
                zMin = Player4.transform.position.z;
            }
            if (div != 2.0f)
            {
                div += 1.0f;
            }

        }

        transform.position = Vector3.Lerp(transform.position, new Vector3((xMax + xMin) / div, transform.position.y, ((zMax + zMin) / div) / 2), 0.1f);
       
    }
    }
    

