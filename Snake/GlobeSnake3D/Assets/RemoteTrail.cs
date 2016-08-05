using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoteTrail : Photon.MonoBehaviour
{
    public static RemoteTrail instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public GameObject segment;

    LinkedList<SegmentScript> segmentList;

    LinkedList<TrailPoint> trailPointList;

    float trailSize;
    float firstTrailSize;
    int num = 0;
    float tailLength;
    Flying flyingDevice;
    // Use this for initialization

    int numOfAir;
    int numOfGround;

    public float getRatio() { return (float)numOfGround / (float)(numOfGround + numOfAir); }
    void Start()
    {
        if (!photonView.isMine)
        {
            flyingDevice = Flying.instance;
            tailLength = firstTrailSize;
            segmentList = new LinkedList<SegmentScript>();
            trailPointList = new LinkedList<TrailPoint>();

            Vector3 newPos = transform.GetChild(0).position;
            Quaternion newRot = transform.GetChild(0).rotation;
            ++num;
            TrailPoint point = new TrailPoint(newPos, newRot, num, newPos.magnitude);
            point.state = SegmentState.GROUND;
            ++numOfGround;
            trailPointList.AddFirst(point);
        }

    }

    bool create = false;

    [PunRPC]
    public void addSegment()
    {
        Debug.Log("addSegment");
        if (photonView.isMine)
            photonView.RPC("addSegment", PhotonTargets.Others);
        else
            create = true;
    }

    public void FixedUpdate()
    {
        if (!photonView.isMine)
        {
            float delta = 0;
            delta = (transform.GetChild(0).position - trailPointList.First.Value.pos).magnitude;
            Vector3 newPos = transform.GetChild(0).position;
            Quaternion newRot = transform.GetChild(0).rotation;
            ++num;

            TrailPoint point = new TrailPoint(newPos, newRot, num, newPos.magnitude);
            if (flyingDevice.isInAir() == true)
            {
                numOfAir++;
                point.state = SegmentState.AIR;
            }
            else
            {
                numOfGround++;
                point.state = SegmentState.GROUND;
            }

            trailPointList.AddFirst(point);


            LinkedListNode<SegmentScript> runner = segmentList.Last; //doesn't matter from which side we start, because we move each at same rate.
            while (runner != null)
            {

                runner.Value.move(delta);
                runner = runner.Previous;
            }

            if (create)
            {
                create_segment();
                create = false;
            }

            trim_tail();
        }
    }
    
    void create_segment()
    {
        Debug.Log("create_segment");
        SegmentScript newSegment = (Instantiate(segment) as GameObject).GetComponent<SegmentScript>();
        newSegment.transform.SetParent(transform);
        newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
        segmentList.AddFirst(newSegment);
        newSegment.set_first(trailPointList.First, tailLength);
        tailLength += trailSize;
    }

    public void set_first_segment_distance(float _firstSegmentDistance)
    {
        firstTrailSize = _firstSegmentDistance;
    }

    public void set_segment_distance(float _segmentDistance)
    {
        trailSize = _segmentDistance;
    }

    void trim_tail()
    {
        if (trailPointList.Last.Value.isTaken() == false)
        {
            if (trailPointList.Last.Value.state == SegmentState.GROUND)
            {
                numOfGround--;
            }
            else
            {
                numOfAir--;
            }
            trailPointList.RemoveLast();
        }
    }
}