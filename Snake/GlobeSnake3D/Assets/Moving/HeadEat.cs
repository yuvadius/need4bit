using UnityEngine;
using System.Collections;

/// <summary>
/// Head eat. Bad name, cause this script is for collision with the head.
/// 
/// responsible for the biting animation, and triggering adding segment events in the trail.
/// 
/// responsible for sending game over message to main controller upon impact with segment.
/// </summary>
public class HeadEat : MonoBehaviour {

	BiteScript bite;
	Trail trail;
    LevelingSystem levelingSystem;

	void Start(){
		bite = GetComponent<BiteScript>();
		trail = transform.parent.gameObject.GetComponent<Trail>();
        levelingSystem = GameObject.FindObjectOfType<LevelingSystem>();
	}

	void OnTriggerEnter(Collider other){
		//if( other.tag == "apple" ){
		//	bite.bite();
		//	trail.addSegment();
  //          levelingSystem.AddApple();
		//}else if( other.tag == "bannana" ){
		//	StartCoroutine(chew(1.5f, Random.Range (1, 3)));
  //          levelingSystem.AddBannana();
		//}else if( other.tag == "segment" ){
		//	GetComponent<Collider>().enabled = false;
		//	MainController.instance.gameOver();
		//}else{
		//	Debug.LogError("Hit something that not supposed to be " + other.tag );
		//}
	}

	IEnumerator chew(float seconds, int times){
		bite.bite();
		float time = seconds;
		float fraction = 0;
		float period = seconds/times;

		while( time > 0 ){
			time -= Time.deltaTime;
			fraction += Time.deltaTime;

			if( fraction > period ){
				bite.bite ();
				fraction = 0;
			}

			yield return null;
		}

		trail.addSegment();
	}
}
