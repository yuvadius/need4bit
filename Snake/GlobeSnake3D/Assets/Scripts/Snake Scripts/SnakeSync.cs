using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour
{
    public Trail trail;
    public float firstSegmentDistance; float prev1;
    public float segmentDistance; float prev2;

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
            //gameObject.AddComponent<PhotonLagSimulationGui>();
            rotationDevice = GameObject.FindGameObjectWithTag("RotationDevice");
            finalSpeeder = FindObjectOfType<RotateForward>();
		}
        else
        {
            globe = GameObject.Find("Game Controllers");
            trail = GetComponentInChildren<Trail>();
            trail.isMine = false;
            if (trail.hasFirst == false)
            {
                trail.SetFirst();
                trail.hasFirst = true;
            }
        }
	}

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            if (firstSegmentDistance != prev1)
            {
                prev1 = firstSegmentDistance;
                trail.set_first_segment_distance(firstSegmentDistance);
            }
            if (segmentDistance != prev2)
            {
                prev2 = segmentDistance;
                trail.set_segment_distance(segmentDistance);
            }
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
            float radius = globe.GetComponent<GlobeController>().globeRadius;
            for (int i = 0; i < segmentLength; i++)
                trail.addSegment();
            for (int i = trailsRot.Length - 1; i >= 0; i--)
            {
                Vector3 position = trailsRot[i] * Vector3.up;
                position = Vector3.ClampMagnitude(position, radius + SnakeController.instance.aboveGlobe);
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
        if (stream.isWriting)
        {
            stream.SendNext(rotationDevice.transform.rotation);
            stream.SendNext(finalSpeeder.degsPerSec);
            stream.SendNext(PhotonNetwork.time);
        }
        else
        {
            Quaternion rotation = (Quaternion)stream.ReceiveNext();
            float speed = (float)stream.ReceiveNext();
            double time = (double)stream.ReceiveNext();
            CustomPayload payload = new CustomPayload(rotation, speed, time);
            if (useExtrapolation)
            {
                Vector3 extrapPoint, emulationPoint;
                Quaternion extrapolateRotation = extrapolater.ExtrapolateFrom(emulator.emulationOffset, payload, out extrapPoint, out emulationPoint);
                emulator.Emulate(extrapolateRotation, extrapPoint, emulationPoint);
            }
        }
    }
}