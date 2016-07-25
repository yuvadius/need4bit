using UnityEngine;
using System.Collections;


/// <summary>
/// This script will create the commet devices
/// </summary>
public class CommetSystem : MonoBehaviour {

	public int commetNumMin, commetNumMax;
	public GameObject commetDevice;

	void Start () {
		int num = Random.Range (commetNumMin, commetNumMax);
		for(int i=0; i< num; ++i){
			GameObject obj = Instantiate(commetDevice) as GameObject;
			obj.transform.parent = transform;
		}
	}

}
