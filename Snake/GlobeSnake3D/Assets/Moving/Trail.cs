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

	public LinkedList<SegmentScript> segmentList = new LinkedList<SegmentScript>();

	public LinkedList<TrailPoint> trailPointList = new LinkedList<TrailPoint>();
	
	public bool hasFirst = false;
	public float gapSize;
	int num = 0;
	float tailLength;

	int create = 0;

	public void myUpdate() {

		addTrailPoint();

		LinkedListNode<SegmentScript> segmentRunner = segmentList.Last;
		LinkedListNode<TrailPoint> trailRunner = trailPointList.First;
		float trailWay = 0; //this is how much circular distance was already covered

		float gapDistanceMultiplier = 2 * Mathf.PI * GlobeSize.instance.radius / 360f;

		while(segmentRunner != null) {

			float gap = gapSize; //this is how much we need to move away from the point

			TrailPoint current = trailRunner.Value;
			TrailPoint next = trailRunner.Next.Value;

			float distance = current.DistanceCircular(next) - trailWay; //this is how much distance is left on this trail segment

			if( gap <= distance) {

			}


			//segmentRunner.Value.move(delta);
			segmentRunner = segmentRunner.Previous;
		}

		if(create > 0) {
			create_segment();
			if(MatchMaker.instance.mySync != null) {
				MatchMaker.instance.mySync.CreateSegment();
			}
			create--;
		}

		trim_tail();
	}

	void addTrailPoint() {
		Vector3 newPos = transform.position;
		Quaternion newRot = transform.rotation;

		TrailPoint point = new TrailPoint(newPos, newRot);
		trailPointList.AddFirst(point);
	}

	public void addSegment() {
		create++;
	}

	public void create_segment() {
		SegmentScript newSegment = Instantiate(segment).GetComponent<SegmentScript>();
		newSegment.transform.SetParent(transform.parent);
		newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
		segmentList.AddFirst(newSegment);
		newSegment.set_first(trailPointList.First, tailLength);
		tailLength += gapSize;
	}

	void trim_tail() {

	}
}