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
			emulator.myUpdate();
            trail.myUpdate();
        }
    }

    
    public void CreateSegment()
    {
		if(photonView.isMine)
			photonView.RPC("RPCAddSegment", PhotonTargets.Others);
		else
			Debug.Log("Should not be here ever, fix it Yhuda!");
		
	}

	[PunRPC]
	public void RPCAddSegment() {
		if(!photonView.isMine)
			trail.AddSegment(false);
		else
			Debug.Log("Should not be here ever, fix it Yhuda!");
	}

    [PunRPC]
    public void CreateTrail(Vector3[] trailPositions, int segmentLength)
    {
        if (!photonView.isMine)
        {
			trail.AddSyncedTrailPoints(trailPositions);

            for (int i = 0; i < segmentLength; i++)
                trail.AddSegment(false);
        }
    }

    public void syncTrail(PhotonPlayer other)
    {
        if (photonView.isMine)
        {
            Vector3[] trailPositions = new Vector3[SnakeController.instance.trail.trailPointList.Count];
            int counter = 0;
            foreach (TrailPoint trailPoint in SnakeController.instance.trail.trailPointList)
            {
                trailPositions[counter] = trailPoint.pos;
                counter++;
            }
            photonView.RPC("CreateTrail", other, trailPositions, SnakeController.instance.trail.segments.Count);
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