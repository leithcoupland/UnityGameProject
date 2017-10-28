using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public GameObject health;
    public GameObject stamina;

    // Use this for initialization
    void Start()
    {

    }



    //Health and stamina changed accoring to player controller information.
    //Player dies when mass is 0;
    // Update is called once per frame
    void Update()
    {
        health.transform.localScale = new Vector3(GetComponent<Rigidbody>().mass, health.transform.localScale.y, health.transform.localScale.z);
        if (GetComponent<Rigidbody>().mass <= 0.01)
        {
            Destroy(this.gameObject);
            AudioManager.instance.PlaySound("death", transform.position);

        }
        stamina.transform.localScale = new Vector3(GetComponent<PlayerController>().getStamina(), stamina.transform.localScale.y, stamina.transform.localScale.z);
    }

    //Damage is taken as player enters lava
    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Fire")
        {
            GetComponent<Rigidbody>().mass -= 0.05f * Time.deltaTime * 1;
        }
    }
}
