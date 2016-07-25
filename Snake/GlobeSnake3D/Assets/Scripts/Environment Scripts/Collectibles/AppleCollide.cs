using UnityEngine;
using System.Collections;

public class AppleCollide : MonoBehaviour {

	Apple me;
	SphereCollider collider2;
	bool isStarted = false;

	void OnEnable(){
		if ( isStarted == false ){
			me = GetComponent<Apple>();
			collider2 = GetComponent<SphereCollider>();
			isStarted = true;
		}

		collider2.enabled = true;
	}

	void OnTriggerEnter(Collider other){
		if( other.tag == "head" ){
			if( isStarted == false ){
				OnEnable();
			}
			me.destroy(other.transform);
            GameObject.FindObjectOfType<UIScore>().AddApple();
			collider2.enabled = false;
		}else{
			Debug.LogError ("Not Head touched Apple");
		}
	}
}
