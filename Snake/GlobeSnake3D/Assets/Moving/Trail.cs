using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailPoint {
	private Vector3 _pos;
	public Vector3 pos {
		get {
			return _pos * GlobeSize.instance.radius;
		}
	}
	public Quaternion rot;

	public TrailPoint(Vector3 _pos, Quaternion _rot) {
		this._pos = _pos.normalized;
		rot = _rot;
	}

	public float DistanceCircular(TrailPoint other) {
		float angle = Vector3.Angle(_pos, other.pos);
		float circumpherence = GlobeSize.instance.circumference;
		return circumpherence * angle / 360f;
	}
}

public class Trail : Photon.MonoBehaviour {

	public GameObject segment;

	public List<SegmentScript> segments = new List<SegmentScript>();

	public LinkedList<TrailPoint> trailPointList = new LinkedList<TrailPoint>();
	
	public bool hasFirst = false;
	public float gapSize;
	int num = 0;
	float tailLength;

	int create = 0;

	public void myUpdate() {

		//first we want to add a new trail point this frame to the snake trail,
		//we should always have at least one before doing anythign else.
		addTrailPoint();

		//we need to make sure we can now run on the trail points and set all the segments
		//properly this frame.
		initTrailRunner();

		//going to go over all the segments and use the trail runner to find them a place to be
		for(int i=0; i<segments.Count; ++i) 
			setSegmentIntoPlace(segments[i]);

		//if we have any segments that need to be created, lets do so
		if( create > 0)
			addSegment();

		//erase any unused and not needed trail points
		trim_tail();
	}

	public void AddSegment() {
		create++;
	}



	#region Trail Runner

	void addTrailPoint() {
		Vector3 newPos = transform.position.normalized;
		Quaternion newRot = transform.rotation;
		TrailPoint point = new TrailPoint(newPos, newRot);
		trailPointList.AddFirst(point);
	}

	LinkedListNode<TrailPoint> trailRunner;

	void initTrailRunner() {
		trailRunner = trailPointList.Last;
    }

	#endregion

	void setSegmentIntoPlace(SegmentScript segment) {

		//move trail runner by gap amount

	}

	void addSegment() {
		create_segment();
		if(MatchMaker.instance.mySync != null) {
			MatchMaker.instance.mySync.CreateSegment();
		}
		create--;
	}

	public void create_segment() {
		SegmentScript newSegment = Instantiate(segment).GetComponent<SegmentScript>();
		newSegment.transform.SetParent(transform.parent);
		newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
		segments.Add(newSegment);
		newSegment.set_first(trailPointList.First, tailLength);
		tailLength += gapSize;
	}

	void trim_tail() {

	}
}