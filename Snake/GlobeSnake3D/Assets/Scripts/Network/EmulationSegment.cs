using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmulationSegment : MonoBehaviour {

	public LinkedListNode<EmulationTrailPoint> before; //before me
	public LinkedListNode<EmulationTrailPoint> after; //after me

	float distanceFromNext = 0;

	public Collider myCollider;
	public bool trailing = true;

	public void set_first(LinkedListNode<EmulationTrailPoint> _before, float distance) {
		before = _before;
		before.Value.take();
		after = null;
		transform.position = before.Value.pos;
		transform.rotation = before.Value.rot;
		distanceFromNext = distance;
	}

	public void move(float distance) {
		myCollider.enabled = false;
		if(trailing == true) {
			distanceFromNext -= distance;
			if(distanceFromNext < 0)//the last segment that entered here is the trailingSegment, no?
			{
				distance = -distanceFromNext;//because trailing means standing in place, dont actually need to move anywhere, just count down is enough
				after = before.Previous;
				trailing = false;
				myCollider.enabled = true;
			} else {
				return;
			}
		}

		while(true) {
			float delta = (after.Value.pos - transform.position).magnitude;
			if(delta >= distance) {
				Vector3 direction = (after.Value.pos - transform.position).normalized * distance;
				transform.Translate(direction, Space.World);

				myCollider.enabled = true;
				return;
			} else {
				go_next();
				distance -= delta;
			}
		}
	}

	void go_next() {
		before.Value.release();
		before = after;
		after = after.Previous;

		before.Value.take();
		transform.position = before.Value.pos;
		transform.rotation = before.Value.rot;
	}
}
