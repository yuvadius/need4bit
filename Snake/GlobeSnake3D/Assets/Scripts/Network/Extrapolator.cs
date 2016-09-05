using UnityEngine;
using System.Collections;

public class Extrapolator : MonoBehaviour {

	public int frames = 40;

	public Transform pivot, sideLever, forwardLever;

	public Transform followingPivot;
	public Transform followingCenter;

	public RotateForward followingForwardRotator;
	public RotateSide followingSideRotatore;
	public ExtrapForward forwardRotator;
	public ExtrapSide sideRotator;

	public GameObject extrapGizmo;

	public void Initialize() {
		pivot.localPosition = pivot.localPosition.normalized * followingPivot.position.magnitude;
	}

	public void ExtrapolateFrom(CustomPayload payload) {
		transform.rotation = payload.quat;
		forwardRotator.degsPerSec = payload.degsPerSecond;

		double deltaTime = PhotonNetwork.time - payload.time;

		int frames = Mathf.RoundToInt(((float)deltaTime / Time.fixedDeltaTime));

		for(int i=0; i< frames; ++i) {
			forwardRotator.myUpdate();
		}

		GameObject newGizmo = Instantiate(extrapGizmo, pivot.position, Quaternion.identity) as GameObject;
		newGizmo.SetActive(true);
	}

	public Vector3 GetExtrapolatedPosition(int frames) {

		transform.rotation = followingCenter.rotation;

        followingForwardRotator.CloneValues(forwardRotator);
		followingSideRotatore.CloneValues(sideRotator);

		for(int i = 0; i < frames; ++i) {
			forwardRotator.myUpdate();
			//sideRotator.myUpdate();
		}

		GameObject newGizmo = Instantiate(extrapGizmo, pivot.position, Quaternion.identity) as GameObject;
		newGizmo.SetActive(true);
		return pivot.position;
	}

}
