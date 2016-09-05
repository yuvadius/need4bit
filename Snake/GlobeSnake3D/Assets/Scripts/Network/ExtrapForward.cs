using UnityEngine;
using System.Collections;

public class ExtrapForward : MonoBehaviour {

	public Transform lever;


	public float degsPerSec;

	public void myUpdate() {
		float angle = degsPerSec * Time.fixedDeltaTime;
		transform.Rotate(lever.localPosition, angle);
	}


}
