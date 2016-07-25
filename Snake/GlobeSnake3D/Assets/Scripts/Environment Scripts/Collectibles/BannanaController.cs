using UnityEngine;
using System.Collections;

/// <summary>
/// Bannana controller. This is responsible for creation and storage of the bannanas.
/// This controller need to be called via the global controller script order
/// This controller needs to set each bannana it creates a point of orbit. which
/// is the globe, thus this controller needs a reference to the globe object.
/// 
/// each frame, it will activate the myUpdate of all Bannana objects, and that will propegate
/// all the messages to AsteroidScript and whatnot.
/// </summary>
using System.Collections.Generic;


public class BannanaController : MonoBehaviour {
	public GameObject bannanaPrefab;
	public Transform pointOfOrbit;
	public float ratePerSecond;

    public int bannanasThisLevel = 10;
	int bannanasCreated = 0;
	int bannanasDestroyed = 0;

	void Start(){
        //bannanasThisLevel = GUIBridge.instance.getLevel();
	}

	List<Bannana> bannanaList = new List<Bannana>();
	

	// Update is called once per frame
	public void myUpdate () {		
		if( bannanasCreated < bannanasThisLevel ){
			if( Time.deltaTime * ratePerSecond > Random.Range(0.0f, 1.0f) ){
				newBannana();
				bannanasCreated++;
			}
		}
		foreach(Bannana bannana in bannanaList){
			bannana.myUpdate();
		}
	}

	private void newBannana(){
			//create bannana, outside of globe, because we dont want to activate a collision upon creation inside the globe
			Bannana bannana = ((GameObject)GameObject.Instantiate(
				bannanaPrefab, 
				new Vector3(100,100,100), 
				Quaternion.identity)).GetComponent<Bannana>();
			bannana.setController(this);
			bannana.setPointOfOrbit(pointOfOrbit);
			bannana.transform.parent = transform;
			bannana.gameObject.name = "Bannana";
			bannanaList.Add(bannana);
		
	}
	
	public void destroy(Bannana bannana){
		bannanaList.Remove(bannana);
		bannanasDestroyed++;
	}
	


}
