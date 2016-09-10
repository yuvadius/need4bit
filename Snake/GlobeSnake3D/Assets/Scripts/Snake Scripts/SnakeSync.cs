using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour
{
    Trail trail;
    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;

    public float delay;
    public Transform snake;
    public Transform remotePivot;
    Quaternion networkRotation;
    Quaternion extrapolateRotation;
    public float rotationInterpolationOffset;
    public bool useExtrapolation;
    GameObject rotationDevice;
    public Transform remoteRotationDevice;
    private RotateForward finalSpeeder;
    public Transform normal;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;

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
            Quaternion rotation = useExtrapolation ? extrapolateRotation : networkRotation;
            remoteRotationDevice.rotation = Quaternion.Lerp(remoteRotationDevice.rotation, rotation, rotationInterpolationOffset);
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
    public Quaternion GetExtrapolatedRotationOffset(Quaternion q1, Quaternion q2, float speed)
    {
        //Quaternion rotation = Quaternion.AngleAxis(GetExtrapolatedAngle(speed), normal.localPosition);
        //return q1 * rotation;
        return q2;

    public float GetExtrapolatedAngle(float speed)
    {
        syncDelay = Time.time - lastSynchronizationTime;
        //syncDelay += (float)PhotonNetwork.GetPing() / 1000f;
        lastSynchronizationTime = Time.time;
        Debug.Log("Speed: " + speed + ", Ping: " + (float)PhotonNetwork.GetPing());
        return speed * ((float)PhotonNetwork.GetPing() / 1000f) * delay;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rotationDevice.transform.rotation);
            stream.SendNext(finalSpeeder.degsPerSec);
        }
        else
        {
            Quaternion rotation = (Quaternion)stream.ReceiveNext();
            float speed = (float)stream.ReceiveNext();
            if (useExtrapolation)
                extrapolateRotation = GetExtrapolatedRotationOffset(networkRotation, rotation, speed);
            networkRotation = rotation;
        }
    }
}