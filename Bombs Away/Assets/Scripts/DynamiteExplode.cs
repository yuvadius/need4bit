using UnityEngine;
using System.Collections;

public class DynamiteExplode : MonoBehaviour {
    public float timeDestroy;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, timeDestroy);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
