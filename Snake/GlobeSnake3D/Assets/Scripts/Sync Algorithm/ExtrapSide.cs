using UnityEngine;
using System.Collections;

public class ExtrapSide : MonoBehaviour {

	public Transform lever;

	public float speed;
	public float turnAngle = 0;

	public void myUpdate(float turn, float frames) {
		turnAngle = -speed * Time.fixedDeltaTime * turn * frames;
		transform.Rotate(lever.localPosition, turnAngle);
	}

}
