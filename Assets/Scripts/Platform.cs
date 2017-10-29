using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	float timer;
	bool alternator = true;
	bool shaking = false;
	bool outOfPlay = false;
	Vector3 originalPos;
	
    public float expireTime = 10f;
	public float expireWarnTime = 0f;
	public bool expired { get; private set; }
	public bool expiringSoon { get; private set; }

	void Start(){
		timer = 0;
		originalPos = gameObject.transform.position;
		expired = false;
		expiringSoon = false;
	}

	void Update (){
		if (outOfPlay) {
			return;
		}

		timer += Time.deltaTime;
		if (timer >= expireTime - expireWarnTime) {
			expiringSoon = true;
			if (!shaking) {
				shaking = true;
				StartCoroutine (Shake ());
			}
		}

		if (timer >= expireTime) {
			expired = true;
			transform.Translate(Vector3.down * Time.deltaTime/8);
			if (timer > expireTime + 10){
				shaking = false;
				outOfPlay = true;
			}
		}
	}

	IEnumerator Shake(){
		while (shaking) {
			float refreshRate = .05f;
			if (alternator) {
				transform.position = new Vector3 (originalPos.x + Random.Range(-0.12f, 0.12f), transform.position.y, originalPos.z + Random.Range(-0.12f, 0.12f));
			} else {
				gameObject.transform.position = new Vector3 (originalPos.x, transform.position.y, originalPos.z);
			}
			alternator = !alternator;
			yield return new WaitForSeconds (refreshRate);
		}
	}
}
