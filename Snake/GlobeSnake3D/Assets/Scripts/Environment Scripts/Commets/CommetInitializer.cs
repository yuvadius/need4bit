using UnityEngine;
using System.Collections;

/// <summary>
/// The task of this script is generate randomness on Start() of the comment, so that
/// they wont all look the same. 
/// </summary>
public class CommetInitializer : MonoBehaviour {
	public float startLifeTimeMin, startLifeTimeMax;
	public float startSpeedMin, startSpeedMax;
	//public float startSizeMin, startSizeMax;
	public float emmisionRateMin, emmisionRateMax;
	//public float shapeAngleMin, shapeAngleMax;
	//public float speedScaleMin, speedScaleMax;

	ParticleSystem system;

	// Use this for initialization
	void Start () {
		system = GetComponent<ParticleSystem>();

		system.startLifetime = Random.Range(startLifeTimeMin, startLifeTimeMax);
		system.startSpeed = Random.Range (startSpeedMin, startSpeedMax);

        system.emissionRate = Random.Range (emmisionRateMin, emmisionRateMax);
	}

}
