using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SegmentState { AIR, GROUND }

public class TrailPoint
{
    public Vector3 pos;
    public Quaternion rot;
    public int num;
    public float height;
	public SegmentState state;
	public int taken;
	public void take(){ taken++; }
	public void release(){ taken--; }
	public bool isTaken(){ return taken > 0; }
    public TrailPoint(Vector3 _pos, Quaternion _rot, int _num, float _height)
    {
		taken = 0;
        pos = _pos;
        rot = _rot;
        num = _num;
        height = _height;
    }
}

public class Trail : Photon.MonoBehaviour
{

    public GameObject segment;

    public LinkedList<SegmentScript> segmentList = new LinkedList<SegmentScript>();

    public LinkedList<TrailPoint> trailPointList = new LinkedList<TrailPoint>();

	public bool hasFirst = false;
    float trailSize;
    int num = 0;
    float tailLength;
	Flying flyingDevice;
    // Use this for initialization

	int numOfAir;
	int numOfGround;

    void Start()
    {
		flyingDevice = Flying.instance;

		if (hasFirst == false)
		{
			SetFirst();
        }
    }

	public void SetFirst()
	{
		Vector3 newPos = transform.position;
		Quaternion newRot = transform.rotation;
		++num;
		TrailPoint point = new TrailPoint(newPos, newRot, num, newPos.magnitude);
		point.state = SegmentState.GROUND;
		++numOfGround;

		trailPointList.AddFirst(point);
		hasFirst = true;
	}

    int create = 0;
    
	public void addSegment(){
		create++;
	}
	
    public void myUpdate()
    {
        float delta = 0;
        delta = (transform.position - trailPointList.First.Value.pos).magnitude;
        Vector3 newPos = transform.position;
        Quaternion newRot = transform.rotation;
        ++num;

		TrailPoint point = new TrailPoint(newPos, newRot, num, newPos.magnitude);
		if( flyingDevice.isInAir() == true ){
			numOfAir++;
			point.state = SegmentState.AIR;
		}else{
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

        if (create > 0)
        {
            create_segment();
            if (MatchMaker.instance.mySync != null)
            {
               MatchMaker.instance.mySync.CreateSegment();
            }
			create--;
        }

        trim_tail();
    }

    public void create_segment()
    {
        SegmentScript newSegment = Instantiate(segment).GetComponent<SegmentScript>();
        newSegment.transform.SetParent(transform.parent);
        newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
        segmentList.AddFirst(newSegment);
        newSegment.set_first(trailPointList.First, tailLength);
        tailLength += trailSize;
    }

    public void set_first_segment_distance(float _firstSegmentDistance)
    {
        tailLength = _firstSegmentDistance;
    }

    public void set_segment_distance(float _segmentDistance)
    {
        trailSize = _segmentDistance;
    }

    void trim_tail()
    {
		if( trailPointList.Last.Value.isTaken() == false ){
			if( trailPointList.Last.Value.state == SegmentState.GROUND ){
				numOfGround--;
			}else{
				numOfAir--;
			}
			trailPointList.RemoveLast();
		}
    }
}