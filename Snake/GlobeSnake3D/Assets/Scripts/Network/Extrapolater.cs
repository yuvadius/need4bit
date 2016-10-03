using UnityEngine;
using System.Collections;

public class Extrapolater : MonoBehaviour
{
    public Transform pivot;
    public ExtrapForward forwardRotator;

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

        float deltaTime = (float)(PhotonNetwork.ServerTimestamp - payload.time) / 1000F;
        Vector3 currentPos = pivot.position;
        float frames = (float)deltaTime / Time.fixedDeltaTime;

        forwardRotator.myUpdate(frames);
        extrapPoint = pivot.position;

		float degreesPerFrame = payload.degsPerSecond * Time.fixedDeltaTime;
        float emulationLagFrames = emulatorOffset / degreesPerFrame;

        forwardRotator.myUpdate(emulationLagFrames);
        emulationPoint = pivot.position;

        return transform.rotation;
    }

	public Quaternion RotateForward(float degsPerSec, float frames) {
		forwardRotator.degsPerSec = degsPerSec;
		forwardRotator.myUpdate(frames);
		return transform.rotation;
	}

}
