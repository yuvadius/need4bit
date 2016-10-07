using UnityEngine;
using System.Collections;

public class GlobeGravity : MonoBehaviour {

	public static GlobeGravity instance;

	[Tooltip("The gravity")]
	public float globeAcceleration;

	void Awake() {
		instance = this;
	}

}
