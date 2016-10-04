using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppleController : MonoBehaviour {

	public GameObject applePrefab;
    private bool CanHaveApples;
	public int maxNumOfApples; int prev1;
	public float ratePerSecond; float prev2;
	float appleHeight;
	public GlobeController globe;

	List<Apple> appleList = new List<Apple>();
	List<Apple> applePool = new List<Apple>();

	// Use this for initialization
	void Start () {
	}

    public void SetCanHaveApples(bool CanHaveApples)
    {
        this.CanHaveApples = CanHaveApples;
    }
	
	// Update is called once per frame
    public void myUpdate()
    {
        if (CanHaveApples)
        {
            appleHeight = globe.globeRadius;
            if (prev1 != maxNumOfApples)
            {
                prev1 = maxNumOfApples;
            }
            if (prev2 != ratePerSecond)
            {
                prev2 = ratePerSecond;
            }

            if (appleList.Count < maxNumOfApples && PhotonNetwork.isMasterClient)
            {
                if (Time.deltaTime * ratePerSecond > Random.Range(0.0f, 1.0f))
                {
                    //create apple
                    Apple apple = null;
                    if (PhotonNetwork.offlineMode)
                    {
                        if (applePool.Count > 0)
                        {
                            apple = applePool[applePool.Count - 1];
                            applePool.RemoveAt(applePool.Count - 1);
                        }
                        else
                        {
                            apple = (GameObject.Instantiate(applePrefab)).GetComponent<Apple>();
                        }
                        apple.gameObject.name = "Apple";
                        appleList.Add(apple);
                        apple.gameObject.SetActive(true);
                    }
                    else
                    {
                        Vector3 randomVector = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
                        randomVector = randomVector.normalized * appleHeight;
                        GameObject appleObject = PhotonNetwork.InstantiateSceneObject("Apple", randomVector, Quaternion.LookRotation(randomVector * 100), 0, null);
                        apple = appleObject.GetComponent<Apple>();
                    }
                    apple.setController(this);
                    apple.transform.parent = transform;
                    appleList.Add(apple);
                }
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
			apple.setHeight(appleHeight, true);
		}
	}
}
