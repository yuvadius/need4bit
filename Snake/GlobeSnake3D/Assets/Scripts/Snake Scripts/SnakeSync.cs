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

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

		if (photonView.isMine)
		{
            //gameObject.AddComponent<PhotonLagSimulationGui>();
            rotationDevice = GameObject.FindGameObjectWithTag("RotationDevice");
            finalSpeeder = FindObjectOfType<RotateForward>();
		}
        else
        {
            trail = GetComponentInChildren<Trail>();
            trail.isMine = false;
        }
	}

    void Start()
    {
        if (!photonView.isMine)
        {
            if (trail.hasFirst == false)
            {
                trail.SetFirst();
                trail.hasFirst = true;
            }

			trail.set_first_segment_distance(firstSegmentDistance);
			trail.set_segment_distance(segmentDistance);
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
    public void CreateSegment(Vector3[] trailPos, Quaternion[] trailRot, Vector3[] segPos, Quaternion[] segRot)
    {
        if (!photonView.isMine)
        {
            for (int i = 0; i < trailPos.Length; i++)
            {
                TrailPoint point = new TrailPoint(trailPos[i], trailRot[i], i, trailPos[i].magnitude, SegmentState.GROUND);
                trail.trailPointList.AddFirst(point);
            }
            for (int i = 0; i < segPos.Length; i++)
                trail.create_segment(segPos[i], segRot[i]);
        }
    }

    public void syncTrail(PhotonPlayer other)
    {
        if (photonView.isMine)
        {
            Vector3[] segPos = new Vector3[SnakeController.instance.trail.segmentList.Count];
            Quaternion[] segRot = new Quaternion[SnakeController.instance.trail.segmentList.Count];
            int counter = 0;
            foreach (SegmentScript segment in SnakeController.instance.trail.segmentList)
            {
                segPos[counter] = segment.transform.position;
                segRot[counter] = segment.transform.rotation;
                counter++;
            }
            Vector3[] trailPos = new Vector3[SnakeController.instance.trail.trailPointList.Count];
            Quaternion[] trailRot = new Quaternion[SnakeController.instance.trail.trailPointList.Count];
            counter = 0;
            foreach (TrailPoint trailPoint in SnakeController.instance.trail.trailPointList)
            {
                trailPos[counter] = trailPoint.pos;
                trailRot[counter] = trailPoint.rot;
                counter++;
            }
            photonView.RPC("CreateSegment", other, trailPos, trailRot, segPos, segRot);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if(stream.isWriting) {
			stream.SendNext(rotationDevice.transform.rotation);
			stream.SendNext(finalSpeeder.degsPerSec);
			stream.SendNext(PhotonNetwork.ServerTimestamp);
		} else {
			Quaternion rotation = (Quaternion)stream.ReceiveNext();
			float speed = (float)stream.ReceiveNext();
			int time = (int)stream.ReceiveNext();
			CustomPayload payload = new CustomPayload(rotation, speed, time);
			if(useExtrapolation) {
				Vector3 extrapPoint, emulationPoint;
				Quaternion extrapolateRotation = extrapolater.ExtrapolateFrom(emulator.emulationOffset, payload, out extrapPoint, out emulationPoint);
				emulator.Emulate(extrapolateRotation, extrapPoint, emulationPoint, speed);
			}
		}
    }
}