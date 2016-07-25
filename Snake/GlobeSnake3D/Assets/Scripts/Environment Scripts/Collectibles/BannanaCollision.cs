using UnityEngine;
using System.Collections;

/// <summary>
/// Bannana bounce. This script will check for collision with the globe, and will propegate,
/// the collisiosn messege to the Bannana script, that will propegate that message, to the
/// Asteroid script that will propel the bannana back into space, pushing off the globes point
/// of impact.
/// 
/// Second Responsibility of this class is to capture collision with the head, in which case
/// the get eatenn event needs to occure, in which the message needs to propegate to the Bannana
/// script which will send the message to the BannanaController to remove this bannana from storage.
/// </summary>
public class BannanaCollision : MonoBehaviour {
	Bannana bannana;

	void Start(){
		bannana = transform.parent.gameObject.GetComponent<Bannana>();
	}


	void OnTriggerEnter(Collider other){
		if( other.tag == "head" ){
			GetComponent<Collider>().enabled = false;
			bannana.eat (other.transform);
            GameObject.FindObjectOfType<UIScore>().AddBannana();
		}else if( other.tag == "globe" ){
			//get propeled back to space.
			bannana.bounce (other.transform);
		}
	}

}
