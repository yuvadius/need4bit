using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppleController : MonoBehaviour {

	public static AppleController instance;

	public GameObject applePrefab;
    private bool CanHaveApples;
	public int maxNumOfApples; int prev1;
	public float ratePerSecond; float prev2;

    public SphereCollider spherCollider;

	public List<Apple> appleList = new List<Apple>();
	List<Apple> applePool = new List<Apple>();

    void Awake()
    {
        if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
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
                    Vector3 randomVector = Apple.AvoidOverlap(GlobeSize.instance.radius, spherCollider.radius) * GlobeSize.instance.radius;
                    GameObject appleObject = PhotonNetwork.InstantiateSceneObject("Apple", randomVector, Quaternion.LookRotation(randomVector * 100), 0, null);
                    apple = appleObject.GetComponent<Apple>();
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
		foreach(Apple apple in appleList) {
			apple.setHeight(GlobeSize.instance.radius, true);
		}
	}
}
