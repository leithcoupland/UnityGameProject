using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Move lava game object up accordingly
	void Update ()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime);
	}
}
