using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterRingSink : MonoBehaviour {

    public float wait = 10f;
    float timer = 0;

    // Use this for initialization
    void Start ()
    {
        
    }
	
    //Move outer rocks of arena downwards over time.
	// Update is called once per frame
	void Update ()
    {
        if(timer>wait)
        {
            this.transform.Translate(Vector3.down * Time.deltaTime/10);
        }

        timer += 1 * Time.deltaTime;

        if (timer>wait*2)
        {
            Destroy(this.gameObject);
        }
        
	}
}
