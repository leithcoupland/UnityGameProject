using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Projectile : MonoBehaviour{

    public float speed;
	public float force = 4;
	public float damage = 5;
    private int playerNo;

    //Set player number of projectile to stop it from colliding with its own player.
    public void setPlayerNo(int _playerNo){
		playerNo = _playerNo;
    }

    //Set speed projectile is translated.
    public void setSpeed(float _speed){
        speed = _speed;
    }

    //Projectile moves forward per frame update at a rate of speed variable.
    //It is then destroyed after certain time.
    void Update(){
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Destroy(this.gameObject, 1);
    }

    //If projectile colides then the other object it forced back with the force specified.
    //Sound is played to indicate this.
    //The object is then destroyed.
	void OnCollisionEnter(Collision _col){
		if (_col.gameObject.tag == "Player" && _col.gameObject.GetComponent<PlayerController>().playerNum != playerNo){
			PlayerController player = _col.gameObject.GetComponent<PlayerController> ();
			Vector3 dir = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            dir = dir.normalized;
			player.Damage (damage);
			player.Push (dir * force);
            Destroy(gameObject);
        }
    }
}
