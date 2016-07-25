using UnityEngine;
using System.Collections;

/// <summary>
/// This script makes the bannana spin. choose random direction and speed and go.
/// </summary>
public class BannanaSpinScript : MonoBehaviour {

	public float spinSpeedMax; //min is zero

	Vector3 randomDirection;
	Vector3 lever;
	float speed;

	void Start(){

		lever = new Vector3(
			Random.Range(-10, 10),
			Random.Range (-10, 10),
			Random.Range (-10, 10)).normalized;

		speed = Random.Range (0, spinSpeedMax);
	}

	void Update(){

		transform.Rotate (lever, speed*Time.deltaTime);

	}

}
