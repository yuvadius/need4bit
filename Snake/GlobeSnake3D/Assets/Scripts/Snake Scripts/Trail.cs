using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailPoint {
	public int trailNum;

	private Vector3 _pos;
	public Vector3 pos {
		get {
			return _pos * GlobeSize.instance.radius;
		}
	}

	public TrailPoint(Vector3 _pos, int trailNum) {
		this._pos = _pos.normalized;
		this.trailNum = trailNum;
    }

	public float DistanceCircular(TrailPoint other) {
		float angle = Vector3.Angle(_pos, other.pos);
		float circumpherence = GlobeSize.instance.circumference;
		return circumpherence * angle / 360f;
	}
}

public class Trail : Photon.MonoBehaviour {

	public TrailRunner trailer;

	public GameObject segment;
	public List<SegmentScript> segments = new List<SegmentScript>();
	public LinkedList<TrailPoint> trailPointList = new LinkedList<TrailPoint>();
	
	public float gapSize;

	int create = 0;

	int trailNum = 0;

#if UNITY_EDITOR
	[Header("Editor Only")]
	public int trailAmount = 0;
#endif

	public void myUpdate() {

		//first we want to add a new trail point this frame to the snake trail,
		//we should always have at least one before doing anythign else.
		addTrailPoint();

		//to not have null references
		if( trailPointList.Count < 5) {
			return;
		}

		//if we have any segments that need to be created, lets do so
		if(create > 0)
			addSegment();

		//we need to make sure we can now run on the trail points and set all the segments
		//properly this frame.
		initTrailRunner();

		//going to go over all the segments and use the trail runner to find them a place to be
		for(int i=0; i<segments.Count; ++i) 
			setSegmentIntoPlace(segments[i]);

		//erase any unused and not needed trail points
		trim_tail();

#if UNITY_EDITOR
		trailAmount = trailPointList.Count;
#endif
	}

	public void AddSegment(bool addLocally) {
		create++;

		if(addLocally) {
			MatchMaker.instance.mySync.CreateSegment();
		}
	}

	#region Trail Runner

	void addTrailPoint() {
		Vector3 newPos = transform.position.normalized;
		TrailPoint point = new TrailPoint(newPos, trailNum++);
		trailPointList.AddFirst(point);		
    }

	public void AddSyncedTrailPoints(Vector3[] positions) {
		trailPointList.Clear(); //reset
        for(int i=0; i<positions.Length; ++i) {
			TrailPoint newPoint = new TrailPoint(
				positions[i],
				trailNum++
			);
            trailPointList.AddLast(newPoint);
		}
	}

	LinkedListNode<TrailPoint> trailRunner;
	float degsAway; //remembers how many degrees between two points set.
	float degToDis; //multiply degrees by this to get the distance
	float degGap; //what is the gap by degrees

	void initTrailRunner() {
		trailRunner = trailPointList.First;
		degsAway = 0;
		degToDis = 2f * Mathf.PI * GlobeSize.instance.radius / 360f;
		degGap = (gapSize * 360f) / (2f * GlobeSize.instance.radius * Mathf.PI);
    }

	void setSegmentIntoPlace(SegmentScript segment) {

		//move trail runner by gap amount
		moveTrailRunner();

		//set its position and rotation to the segment
		segment.transform.position = trailer.pivot.position;
		segment.transform.rotation = trailer.pivot.rotation;
	}

	void moveTrailRunner() {

		float gapToCover = degGap;

		int count = 0;
		while(true) {
			if( trailRunner.Next.Next == null) {
				break;
			}

			//TODO Optimization: keep ang between two positions in a cache
			float remainder = Vector3.Angle(trailRunner.Value.pos, trailRunner.Next.Value.pos) - degsAway;

			if(remainder >= gapToCover) {
				degsAway += gapToCover;
				break;
			} else {
				degsAway = 0;
				gapToCover -= remainder;
				count++;
				trailRunner = trailRunner.Next;
			}
		}

		trailer.Move(trailRunner.Value.pos, trailRunner.Next.Value.pos, degsAway);
	}

	#endregion

	void addSegment() {
		create_segment().myCollider.enabled = false; //should be turned off for me
		create--;
	}


	public SegmentScript create_segment() {
		SegmentScript newSegment = Instantiate(segment).GetComponent<SegmentScript>();
		newSegment.transform.SetParent(transform.parent);
		newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
		segments.Add(newSegment);
		return newSegment;
	}

	void trim_tail() {
		int limit;

		limit = trailRunner.Next.Value.trailNum;

		while(trailPointList.Last.Value.trailNum < limit - 2) {//taking a 2 trail offset just in case
			trailPointList.RemoveLast();
		}
	}
}