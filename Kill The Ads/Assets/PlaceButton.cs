using UnityEngine;
using System.Collections;

public class PlaceButton : MonoBehaviour {
    public GameObject ad;
	// Use this for initialization
	void Start () {
        //transform.position = new Vector3(ad.position.x, ad.position.y);
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0, 0);
    }
}
