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

	

	public float height;

	public void Initialize() {
		pivot.localPosition = pivot.localPosition.normalized * followingPivot.position.magnitude;
		height = pivot.position.magnitude;
	}

	public Quaternion ExtrapolateFrom(float emulatorOffset, CustomPayload payload, out Vector3 extrapPoint, out Vector3 emulationPoint) {

		transform.rotation = payload.quat;
		forwardRotator.degsPerSec = payload.degsPerSecond;

		double deltaTime = PhotonNetwork.time - payload.time;
		Vector3 currentPos = pivot.position;
		float frames = (float)deltaTime / Time.fixedDeltaTime;

		forwardRotator.myUpdate(frames);
		extrapPoint = pivot.position;

		float distancePerFrame = (currentPos - pivot.position).magnitude / frames;
		float emulationLagFrames = emulatorOffset / distancePerFrame;

		forwardRotator.myUpdate(emulationLagFrames);
		emulationPoint = pivot.position;

		return transform.rotation;
	}

}
