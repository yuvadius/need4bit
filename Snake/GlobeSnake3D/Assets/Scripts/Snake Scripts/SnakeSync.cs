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
    Quaternion networkRotation;
    public float rotationInterpolationOffset;
    public bool useExtrapolation;
    GameObject rotationDevice;
    public Transform remoteRotationDevice;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

		if (!photonView.isMine)
		{
			trail = GetComponentInChildren<Trail>();
			trail.isMine = false;
		}
	}

    void Start()
    {
        if (photonView.isMine)
        {
            //gameObject.AddComponent<PhotonLagSimulationGui>();
            rotationDevice = GameObject.FindGameObjectWithTag("RotationDevice");
        }
        else
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
            remoteRotationDevice.rotation = Quaternion.Lerp(remoteRotationDevice.rotation, networkRotation, rotationInterpolationOffset);
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

    /// <summary>
    /// Calculates an estimated rotation based on the last synchronized rotation,
    /// the time when the last rotation was received and the movement speed of the object
    /// </summary>
    /// <param name="rotation">The received rotation from photon</param>
    /// <returns>Estimated rotation of the remote object</returns>
    public Quaternion GetExtrapolatedRotationOffset(Quaternion rotation)
    {
        syncDelay = Time.time - lastSynchronizationTime;
        //syncDelay += (float)PhotonNetwork.GetPing() / 1000f;
        lastSynchronizationTime = Time.time;
        return Quaternion.identity;

        /*Quaternion rot = q2 * Quaternion.Inverse(q1);
        double dt = (t3 - t1) / (t2 - t1);
        float ang = 0.0f;
        Vector3 axis = Vector3.zero;
        rot.ToAngleAxis(out ang, out axis);
        if (ang > 180)
            ang -= 360;
        ang = ang * (float)dt % 360;
        q3 = Quaternion.AngleAxis(ang, axis) * q1;*/
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rotationDevice.transform.rotation);
        }
        else
        {
            networkRotation = (Quaternion)stream.ReceiveNext();
            if (useExtrapolation)
            {
                networkRotation = GetExtrapolatedRotationOffset(networkRotation);
            }
        }
    }
}