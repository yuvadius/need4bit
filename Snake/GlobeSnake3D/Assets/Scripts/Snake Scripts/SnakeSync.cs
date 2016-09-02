using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeSync : Photon.MonoBehaviour
{
    Trail trail;
    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;
    public RemoteFowardRotate remoteDevice;

	public RotateForward finalSpeeder;

    public float positionInterpolationOffset;
    public float rotationInterpolationOffset;
    public bool lerpRotation;
    public bool useExtrapolation;
    public float delay;
    private bool firstSync = false;
    Queue<Vector3> oldNetworkPositions = new Queue<Vector3>();
    public int extrapolateNumberOfStoredPositions;
    float networkSpeed;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 networkPosition = Vector3.zero;
    private Vector3 extrapolatedNetworkPosition = Vector3.zero;
    private Quaternion networkRotation = Quaternion.identity;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

		if (!photonView.isMine)
		{
			trail = GetComponentInChildren<Trail>();
			trail.isMine = false;
		} else {
			finalSpeeder = FindObjectOfType<RotateForward>();
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
        if (photonView.isMine)
        {
            transform.GetChild(0).position = LocalSnake.instance.transform.GetChild(0).position;
            transform.GetChild(0).rotation = LocalSnake.instance.transform.GetChild(0).rotation;
        }
        /*else if (!remoteDevice)
        {
            syncTime += Time.deltaTime;
            transform.GetChild(0).position = Vector3.Slerp(transform.GetChild(0).position, networkPosition, 0.1f);
            transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, networkRotation, syncTime / syncDelay);
        }*/
    }

    void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            if (!remoteDevice && firstSync)
            {
                Vector3 position = (useExtrapolation) ? extrapolatedNetworkPosition : networkPosition;
                transform.GetChild(0).position = Vector3.Slerp(transform.GetChild(0).position, position, positionInterpolationOffset);
                if (lerpRotation)
                    transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, networkRotation, rotationInterpolationOffset);
                else
                    transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, networkRotation, rotationInterpolationOffset);
            }
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

    Vector3 GetOldestStoredNetworkPosition()
    {
        Vector3 oldPosition = networkPosition;
        if (oldNetworkPositions.Count > 0)
        {
            oldPosition = oldNetworkPositions.Peek();
        }
        return oldPosition;
    }

    /// <summary>
    /// Calculates an estimated position based on the last synchronized position,
    /// the time when the last position was received and the movement speed of the object
    /// </summary>
    /// <param name="position">The received position from photon</param>
    /// <returns>Estimated position of the remote object</returns>
    public Vector3 GetExtrapolatedPositionOffset(Vector3 position)
    {
        syncDelay = Time.time - lastSynchronizationTime;
        //syncDelay += (float)PhotonNetwork.GetPing() / 1000f;
        lastSynchronizationTime = Time.time;

        float extrapolateAngle = Vector3.Angle(networkPosition, GetOldestStoredNetworkPosition());
        extrapolateAngle = extrapolateAngle * (1 / syncDelay);

        float angle = Vector3.Angle(transform.GetChild(0).position, networkPosition);
        Vector3 normal = Vector3.Cross(transform.GetChild(0).position, networkPosition);
        Quaternion rotation = Quaternion.AngleAxis(GetExtrapolatedAngle(), normal);
        Debug.Log(angle);
		Vector3 extrapolatePosition = rotation * position;
        return extrapolatePosition;
    }

    public float GetExtrapolatedAngle()
    {
		float val = ((float)PhotonNetwork.GetPing() / 1000f) * networkSpeed * delay;
		print("Val: " + val + " Lagg: " + syncDelay + " speed: " + networkSpeed);
		return val;
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).position);
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).rotation);
			stream.SendNext(finalSpeeder.degsPerSec);
        }
        else
        {
            Vector3 readPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
			networkSpeed = (float)stream.ReceiveNext();
            if (!firstSync)
            {
                transform.GetChild(0).position = networkPosition;
                transform.GetChild(0).rotation = networkRotation;
                firstSync = true;
            }

            if (useExtrapolation)
            {
                if (oldNetworkPositions.Count == 0)
                {
                    // if we don't have old positions yet, this is the very first update this client reads. let's use this as current AND old position.
                    networkPosition = readPosition;
                }
                // the previously received position becomes the old(er) one and queued. the new one is the m_NetworkPosition
                oldNetworkPositions.Enqueue(networkPosition);
                networkPosition = readPosition;
                // reduce items in queue to defined number of stored positions.
                while (oldNetworkPositions.Count > extrapolateNumberOfStoredPositions)
                {
                    oldNetworkPositions.Dequeue();
                }
                extrapolatedNetworkPosition = GetExtrapolatedPositionOffset(networkPosition);
            }
            else
                networkPosition = readPosition;

            if (remoteDevice)
                remoteDevice.ManageDestination(networkPosition);
        }
    }
}