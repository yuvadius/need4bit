using UnityEngine;
using System.Collections;

/// <summary>
/// The task of this script is to choose a random hight and direction, start Spin, and spin speed
/// And then just keep on spining.
/// </summary>
public class CommetDevice : MonoBehaviour {
	public float heightMin, heightMax;
	public float speedMin, speedMax;

	public GameObject redCommet;
	public GameObject greenCommet;
	public GameObject blueCommet;

	public Transform directionLever;
	public Transform spinLever;

	float height, speed;

	void Start(){
		height = Random.Range(heightMin, heightMax);
		speed = Random.Range (speedMin, speedMax);
		int num = Random.Range (0, 3);
		GameObject commet = null;
		switch( num ){
		case 0: 
			commet = Instantiate(redCommet, new Vector3(0, height, 0), transform.rotation) as GameObject;
			break;
		case 1:
			commet = Instantiate(greenCommet, new Vector3(0, height, 0), transform.rotation) as GameObject;
			break;
		case 2:
			commet = Instantiate(blueCommet, new Vector3(0, height, 0), transform.rotation) as GameObject;
			break;
		}
		commet.transform.parent = transform;

		//now to take a direction spin
		transform.Rotate(directionLever.localPosition, Random.Range (0, 360));

		//now to take a random starting point
		transform.Rotate (spinLever.localPosition, Random.Range(0, 360));
	}

	void Update(){
		float distance = speed * Time.deltaTime;
		float angle = distance * 360 / (2 * Mathf.PI * height);
		transform.Rotate (spinLever.localPosition, angle);
	}
}
