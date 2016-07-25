using UnityEngine;
using System.Collections;


/// <summary>
/// Capacity controller. 
/// </summary>
public class CapacityController : MonoBehaviour {
	public static CapacityController instance;
	void Awake(){
		if( instance == null ){
			instance = this;
		}
	}

	float capacity;
	public int maxCapacity;
	//public float additionOfCapacity;
	public void addApple(){
		capacity += 10;
		capacity = capacity > maxCapacity ? maxCapacity : capacity;
	}
	public void addBannana(){
		capacity += 100;
	}
	public float regenerationRate;
	public float degenerationRate;
	public int minFlyCapacity;
	public float getCapacity(){ return capacity; }
	public float getFlyStrength(){
        return 1;  
	}

	void Start(){
		capacity = 0;
        maxCapacity += 10;
	}

	public void myUpdate(){
		if( capacity < maxCapacity ){
			capacity += Time.deltaTime * regenerationRate * Trail.instance.getRatio();
		}
		if( Trail.instance.getRatio() == 0 ){
			capacity -= Time.deltaTime * degenerationRate;
		}
	}


}
