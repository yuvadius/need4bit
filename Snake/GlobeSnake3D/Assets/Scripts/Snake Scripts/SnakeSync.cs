using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour
{
    Trail trail;
    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;

    public Transform snake;
    public Transform remotePivot;
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
        }
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            snake.position = remotePivot.position;
            snake.rotation = remotePivot.rotation;
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
    public void CreateSegment(Vector3[] positions, Quaternion[] rotations)
    {
		if (!photonView.isMine)
			for (int i = 0; i < positions.Length; i++)
				trail.addSegment();
    }

    public void CreateSegment(PhotonPlayer other, Vector3[] positions, Quaternion[] rotations)
    {
        if (photonView.isMine)
            photonView.RPC("CreateSegment", other, positions, rotations);
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