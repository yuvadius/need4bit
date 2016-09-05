using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmulationTrailPoint {

	public Vector3 pos;
	public Quaternion rot;
	public int num;
	public int taken;
	public void take() { taken++; }
	public void release() { taken--; }
	public bool isTaken() { return taken > 0; }

	public EmulationTrailPoint(Vector3 _pos, Quaternion _rot, int _num) {
		taken = 0;
		pos = _pos;
		rot = _rot;
		num = _num;
	}

}

public class EmulationTrail : MonoBehaviour {

	public GameObject segment;

	public LinkedList<EmulationSegment> segmentList = new LinkedList<EmulationSegment>();

	public LinkedList<EmulationTrailPoint> trailPointList = new LinkedList<EmulationTrailPoint>();

	bool hasFirst = false;
	float trailSize;
	int num = 0;
	float tailLength;
	int create = 0;

	void Awake() {
		//set up trailSize and tailLength
		trailSize = FindObjectOfType<SnakeController>().segmentDistance;
		tailLength = FindObjectOfType<SnakeController>().firstSegmentDistance;
	}


	public void myUpdate() {
		if(hasFirst == false) {
			SetFirst();
		}

		float delta = 0;
		delta = (transform.position - trailPointList.First.Value.pos).magnitude;
		Vector3 newPos = transform.position;
		Quaternion newRot = transform.rotation;

		++num;

		EmulationTrailPoint point = new EmulationTrailPoint(newPos, newRot, num);

		trailPointList.AddFirst(point);


		LinkedListNode<EmulationSegment> runner = segmentList.Last; //doesn't matter from which side we start, because we move each at same rate.
		while(runner != null) {
			runner.Value.move(delta);
			runner = runner.Previous;
		}

		if(create > 0) {
			create_segment();
			create--;
		}

		trim_tail();
	}


	public void AddSegment() {
		create++;
	}

	public void SetFirst() {
		Vector3 newPos = transform.position;
		Quaternion newRot = transform.rotation;
		++num;
		EmulationTrailPoint point = new EmulationTrailPoint(newPos, newRot, num);


		trailPointList.AddFirst(point);
		hasFirst = true;
	}

	public void create_segment() {
		EmulationSegment newSegment = Instantiate(segment).GetComponent<EmulationSegment>();
		newSegment.transform.SetParent(transform.parent.parent.parent);
		newSegment.gameObject.SetActive(true);
		newSegment.name = "Segment " + newSegment.transform.GetSiblingIndex();
		segmentList.AddFirst(newSegment);
		newSegment.set_first(trailPointList.First, tailLength);
		tailLength += trailSize;
	}

	void trim_tail() {
		if(trailPointList.Last.Value.isTaken() == false) {
			trailPointList.RemoveLast();
		}
	}

}
