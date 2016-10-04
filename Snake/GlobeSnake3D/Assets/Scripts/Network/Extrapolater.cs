using UnityEngine;
using System.Collections;

public class Extrapolater : MonoBehaviour
{
    public Transform pivot;
    public ExtrapForward forwardRotator;
	public ExtrapSide sideRotator;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="emulatorOffset">How many degrees away I am from the emulation point towards which my rotation is lerping to</param>
	/// <param name="payload"></param>
	/// <param name="extrapPoint"></param>
	/// <param name="emulationPoint"></param>
	/// <returns></returns>
    public Quaternion ExtrapolateFrom(float emulatorOffset, CustomPayload payload, out Vector3 extrapPoint, out Vector3 emulationPoint){
        transform.rotation = payload.quat;
        forwardRotator.degsPerSec = payload.degsPerSecond;

		if(BenchmarkController.instance.shouldRotateTest) {
			BenchmarkController.instance.CreateLandmark(transform.rotation, 1);
		}

		double deltaTime = PhotonNetwork.time - payload.time;
        Vector3 currentPos = pivot.position;
        float frames = (float)deltaTime / Time.fixedDeltaTime;

		extrap(frames, payload);

        extrapPoint = pivot.position;

		if(BenchmarkController.instance.shouldRotateTest) {
			BenchmarkController.instance.CreateLandmark(transform.rotation, 2);
		}

		float degreesPerFrame = payload.degsPerSecond * Time.fixedDeltaTime;
        float emulationLagFrames = emulatorOffset / degreesPerFrame;
		extrap(emulationLagFrames, payload);

		emulationPoint = pivot.position;

		if(BenchmarkController.instance.shouldRotateTest) {
			BenchmarkController.instance.CreateLandmark(transform.rotation, 3);
		}
		return transform.rotation;
    }

	void extrap(float frames, CustomPayload payload) {
		int frameNum = (int)frames;
		float remainder = frames - (float)frameNum;

		for(int i = 0; i < frameNum; ++i) {
			payload.horizontalMove = InputDriver.instance.HorizontalUpdate(payload.horizontalAim, payload.horizontalMove, 1f);
			sideRotator.myUpdate(payload.horizontalMove, 1);
			forwardRotator.myUpdate(1);
		}

		payload.horizontalMove = InputDriver.instance.HorizontalUpdate(payload.horizontalAim, payload.horizontalMove, remainder);
		sideRotator.myUpdate(payload.horizontalMove, remainder);
		forwardRotator.myUpdate(remainder);
	}

	public Quaternion RotateForward(float degsPerSec, float frames) {
		forwardRotator.degsPerSec = degsPerSec;
		forwardRotator.myUpdate(frames);
		return transform.rotation;
	}

}
