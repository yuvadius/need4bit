using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppleController : MonoBehaviour {

	public GameObject applePrefab;

	public int maxNumOfApples; int prev1;
	public float ratePerSecond; float prev2;
	float appleHeight;
	public GlobeController globe;

	List<Apple> appleList = new List<Apple>();
	List<Apple> applePool = new List<Apple>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public void myUpdate () {
		appleHeight = globe.globeRadius;
		if( prev1 != maxNumOfApples ){
			prev1 = maxNumOfApples;
		}
		if( prev2 != ratePerSecond ){
			prev2 = ratePerSecond;
		}

		if( appleList.Count < maxNumOfApples ){
			if( Time.deltaTime * ratePerSecond > Random.Range(0.0f, 1.0f) ){
				//create apple
				Apple apple = null;
				if( applePool.Count > 0 ){
					apple = applePool[applePool.Count-1];
					applePool.RemoveAt(applePool.Count-1);
				}else{
					apple = (GameObject.Instantiate(applePrefab)).GetComponent<Apple>();
				}
				apple.setController(this);
				apple.setHeight(appleHeight);
				apple.transform.parent = transform;
				apple.gameObject.name = "Apple";
				appleList.Add(apple);
                apple.gameObject.SetActive(true);
			}
		}
	}

	public void destroy(Apple apple){
		appleList.Remove(apple);
	}

	public void addAppleToPool(Apple apple){
		applePool.Add(apple);
		apple.gameObject.SetActive(false);
	}

	public void setHeight(float globeHeight){
		appleHeight = globeHeight;
		resetAppleHeight();
	}

	void resetAppleHeight(){
		foreach(Apple apple in appleList){
			apple.setHeight(appleHeight);
		}
	}
}
