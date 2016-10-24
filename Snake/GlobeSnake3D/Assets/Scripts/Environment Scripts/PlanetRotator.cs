using UnityEngine;
using System.Collections;

public class PlanetRotator : MonoBehaviour {

	public static PlanetRotator instance;

	public bool shouldSpin = true;
	public MeshRenderer globeRenderer;

	Material globeMat;

	void Awake() {
		instance = this;
		//This is so that git will not constantly want to commit the material of the globe
#if UNITY_EDITOR
		globeMat = globeRenderer.material;
#else
		globeMat = globeRenderer.sharedMaterial;
#endif
	}

	public void SetTime(float time) {
		if(shouldSpin) {
			globeMat.SetFloat("_ManualTime", time);
		}
	}

}
