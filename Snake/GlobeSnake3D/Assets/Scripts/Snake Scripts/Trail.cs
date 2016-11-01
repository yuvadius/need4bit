using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailPoint {
	public int trailNum;
	public GameObject trailGizmo;
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
}

public class Trail : Photon.MonoBehaviour {

	public TrailRunner trailer;
	public bool isMine;
	public bool shouldShowTrail;
    public bool selfCollide;//Can the snake collide with itself
	public GameObject trailGizmo;
	public GameObject segment;
	public List<SegmentScript> segments = new List<SegmentScript>();
	public LinkedList<TrailPoint> trailPointList = new LinkedList<TrailPoint>();
	
	public float gapSize;
	private bool isSet;
	int create = 0;

	int trailNum = 0;

#if UNITY_EDITOR
	[Header("Editor Only")]
	public int trailAmount = 0;
#endif

	public void myUpdate() {
		if( !isMine && !isSet) 
			return;
		
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
			setSegmentIntoPlace(segments[i], i);

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
		} else {
			isSet = true; //TODO: remove the necessety for isSet by having all initialize remote snake come through one waypoint
		}
	}

	#region Trail Runner

	void addTrailPoint() {
		Vector3 newPos = transform.position.normalized;
		TrailPoint point = new TrailPoint(newPos, trailNum++);
		trailPointList.AddFirst(point);
		if( shouldShowTrail) {
			GameObject newTrailGizmo = Instantiate(trailGizmo) as GameObject;
			newTrailGizmo.transform.SetParent(null);
			newTrailGizmo.name = trailNum.ToString();
			newTrailGizmo.transform.position = newPos;
			newTrailGizmo.transform.LookAt(Vector3.zero);
		}		
    }

	public void AddSyncedTrailPoints(Vector3[] positions) {
		trailPointList.Clear(); //reset
        for(int i=0; i<positions.Length; ++i) {
			TrailPoint newPoint = new TrailPoint(
				positions[i],
				trailNum++
			);
			if(shouldShowTrail) {
				GameObject newTrailGizmo = Instantiate(trailGizmo) as GameObject;
				newTrailGizmo.transform.SetParent(null);
				newTrailGizmo.name = trailNum.ToString();
				newTrailGizmo.transform.position = positions[i];
				newTrailGizmo.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
				newTrailGizmo.transform.LookAt(Vector3.zero);
			}
			trailPointList.AddLast(newPoint);
		}
		isSet = true;
	}

	LinkedListNode<TrailPoint> trailRunner;
	float degsAway; //remembers how many degrees between two points set.
	float degGap; //what is the gap by degrees

	void initTrailRunner() {
		trailRunner = trailPointList.First;
		degsAway = 0;
		degGap = (gapSize * 360f) / (2f * GlobeSize.instance.radius * Mathf.PI);
    }

	void setSegmentIntoPlace(SegmentScript segment, int count) {

		//move trail runner by gap amount
		if( moveTrailRunner() ){
			segment.myCollider.enabled = count > 2 || !isMine;
		} else {
			segment.myCollider.enabled = !isMine;
        }

		//set its position and rotation to the segment
		segment.transform.position = trailer.pivot.position;
		segment.transform.rotation = trailer.pivot.rotation;
	}
	
	/// <summary>
	/// moves the trail runner by a gap amount, and sets the trailer at the position and rotation of the next segment
	/// </summary>
	/// <returns> if the trailer was successfull in procedding by gap amount, or did trail ended </returns>
	bool moveTrailRunner() {

		bool result = true;
		float gapToCover = degGap;

		int count = 0;
		while(true) {
			if( trailRunner.Next.Next == null) {
				result = false;
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
		return result;
	}

	#endregion

	void addSegment() {
		create_segment();
		create--;
	}


	public SegmentScript create_segment() {
		SegmentScript newSegment = Instantiate(segment).GetComponent<SegmentScript>();
		newSegment.transform.SetParent(transform.parent);
		newSegment.transform.position = Vector3.zero;
        segments.Add(newSegment);
        newSegment.name = "Segment " + segments.Count;//Start segment count from 1
        if ((segments.Count > 2 && selfCollide) || !isMine)
            newSegment.myCollider.enabled = true;
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