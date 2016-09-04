using UnityEngine;
using System.Collections;

public class ExtrapSide : MonoBehaviour {

	public InputDriver inputDriver;
	public Transform lever;

	public float speed;
	public float turnAngle = 0;

	public void myUpdate() {
		float turn = inputDriver.horizontalMove;
		turnAngle = -speed * Time.fixedDeltaTime * turn;
		transform.Rotate(lever.localPosition, turnAngle);
	}

}
