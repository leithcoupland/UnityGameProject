using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Projectile : MonoBehaviour
{

    public float speed;
    private int playerNo;

    void Start()
    {
    }

    //Set player number of projectile to stop it from colliding with its own player.
    public void setPlayerNo(int x)
    {
        playerNo = x;
    }

    //Set speed projectile is translated.
    public void setSpeed(float _speed)
    {
        speed = _speed;
    }

    //Projectile moves forward per frame update at a rate of speed variable.
    //It is then destroyed after certain time.
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Destroy(this.gameObject, 1);
    }




    //If projectile colides then the other object it forced back with the force specified.
    //Sound is played to indicate this.
    //The object is then destroyed.
    void OnCollisionEnter(Collision c)
    {
        float force = 400;

        if (c.gameObject.tag == "Player" && c.gameObject.GetComponent<PlayerController>().playerNum != playerNo)
        {
            AudioManager.instance.PlaySound("grunt", c.transform.position);
            Vector3 dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
            c.gameObject.GetComponent<Rigidbody>().mass = c.gameObject.GetComponent<Rigidbody>().mass - 0.05f;
            if (c.gameObject.GetComponent<Rigidbody>().mass <= 0.01)
            {
                Destroy(c.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
