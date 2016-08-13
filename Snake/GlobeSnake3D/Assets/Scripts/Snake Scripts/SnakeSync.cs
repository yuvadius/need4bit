﻿using UnityEngine;
using System.Collections;

public class SnakeSync : Photon.MonoBehaviour
{
    public SnakeColorControl colorControl;

    Trail trail;

    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

		if (!photonView.isMine)
		{
			trail = GetComponentInChildren<Trail>();
			trail.isMine = false;
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
    public void SetPlayerColor(SnakeColor color)
    {
        colorControl.SetColor(color);

        if (photonView.isMine)
            photonView.RPC("SetPlayerColor", PhotonTargets.OthersBuffered, color);
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
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).position);
            stream.SendNext(LocalSnake.instance.transform.GetChild(0).rotation);
        }
        else
        {
            transform.GetChild(0).position = (Vector3)stream.ReceiveNext();
            transform.GetChild(0).rotation = (Quaternion)stream.ReceiveNext();
			if (trail.hasFirst == false)
			{
				trail.SetFirst();
				trail.hasFirst = true;
			}
        }
    }
}
