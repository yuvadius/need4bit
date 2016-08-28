using UnityEngine;
using System.Collections;

public class SnakeSync : Photon.MonoBehaviour
{
    //bruce lee showered here
    Trail trail;
    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;
    public RemoteFowardRotate remoteDevice;

    public float positionInterpolationOffset;
    public float rotationInterpolationOffset;
    public bool lerpRotation;
    public bool useExtrapolation;
    public float delay;
    private bool firstSync = false;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 networkPosition = Vector3.zero;
    private Quaternion networkRotation = Quaternion.identity;

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
                transform.GetChild(0).position = Vector3.Slerp(transform.GetChild(0).position, networkPosition, positionInterpolationOffset);
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

        float angle = Vector3.Angle(transform.GetChild(0).position, networkPosition);
        Vector3 normal = Vector3.Cross(transform.GetChild(0).position, networkPosition);
        Quaternion rotation = Quaternion.AngleAxis(angle * syncDelay * delay, normal);
        Vector3 extrapolatePosition = rotation * position;
        return extrapolatePosition;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).position);
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            if (!firstSync)
            {
                transform.GetChild(0).position = networkPosition;
                transform.GetChild(0).rotation = networkRotation;
                firstSync = true;
            }
            if (useExtrapolation)
                networkPosition = GetExtrapolatedPositionOffset(networkPosition);
            if (remoteDevice)
            {
                remoteDevice.ManageDestination(networkPosition);
            }
        }
    }
}
