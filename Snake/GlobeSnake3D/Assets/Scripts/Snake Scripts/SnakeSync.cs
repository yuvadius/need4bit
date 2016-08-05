using UnityEngine;
using System.Collections;

public class SnakeSync : Photon.MonoBehaviour
{
    public static SnakeSync instance;
    public GameObject segment;
    Trail trail;

    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        trail = GetComponentInChildren<Trail>();
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
            trail.create_segment(false);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.parent.position);
            stream.SendNext(transform.parent.rotation);
            stream.SendNext(transform.parent.localScale);
        }
        else
        {
            transform.GetChild(0).position = (Vector3)stream.ReceiveNext();
            transform.GetChild(0).rotation = (Quaternion)stream.ReceiveNext();
            transform.GetChild(0).localScale = (Vector3)stream.ReceiveNext();
        }
    }
}
