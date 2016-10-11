using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour
{
    public Trail trail;
    public float firstSegmentDistance;
    public float segmentDistance;

    public bool useExtrapolation;

    public Extrapolater extrapolater;
    public RotationDeviceEmulator emulator;
    GameObject rotationDevice;
    RotateForward finalSpeeder;
    GameObject globe;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

		if (photonView.isMine)
		{
            rotationDevice = GameObject.FindGameObjectWithTag("RotationDevice");
            finalSpeeder = FindObjectOfType<RotateForward>();
		}
        else
        {
            globe = GameObject.Find("Game Controllers");
            trail = GetComponentInChildren<Trail>();
		}
	}

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            trail.myUpdate();
        }
    }

    [PunRPC]
    public void CreateSegment()
    {
        if (photonView.isMine)
            photonView.RPC("CreateSegment", PhotonTargets.Others);
        else
            trail.create_segment();
    }

    [PunRPC]
    public void CreateTrail(Quaternion[] trailsRot, int segmentLength)
    {
        if (!photonView.isMine)
        {
            for (int i = 0; i < segmentLength; i++)
                trail.addSegment();
            for (int i = trailsRot.Length - 1; i >= 0; i--)
            {
                Vector3 position = trailsRot[i] * Vector3.up;
                position = Vector3.ClampMagnitude(position, GlobeSize.instance.radius);
                transform.GetChild(0).position = position;
                transform.GetChild(0).rotation = trailsRot[i];
                trail.myUpdate();
            }
        }
    }

    public void syncTrail(PhotonPlayer other)
    {
        if (photonView.isMine)
        {
            Quaternion[] trailsRot = new Quaternion[SnakeController.instance.trail.trailPointList.Count];
            int counter = 0;
            foreach (TrailPoint trailPoint in SnakeController.instance.trail.trailPointList)
            {
                trailsRot[counter] = trailPoint.rot;
                counter++;
            }
            photonView.RPC("CreateTrail", other, trailsRot, SnakeController.instance.trail.segmentList.Count);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if(stream.isWriting) {
			stream.SendNext(rotationDevice.transform.rotation);
			stream.SendNext(finalSpeeder.degsPerSec);
			stream.SendNext(InputDriver.instance.horizontalAim);
			stream.SendNext(InputDriver.instance.horizontalMove);
		} else {
			Quaternion rotation = (Quaternion)stream.ReceiveNext();
			float speed = (float)stream.ReceiveNext();
			float horizontalAim = (float)stream.ReceiveNext();
			float horizontalMove = (float)stream.ReceiveNext();
			CustomPayload payload = new CustomPayload(rotation, speed, info.timestamp, horizontalAim, horizontalMove);
			if(useExtrapolation) {
				Vector3 extrapPoint, emulationPoint;
				Quaternion extrapolateRotation = extrapolater.ExtrapolateFrom(emulator.emulationOffset, payload, out extrapPoint, out emulationPoint);
				emulator.Emulate(extrapolateRotation, extrapPoint, emulationPoint, speed);
			}
		}
    }
}