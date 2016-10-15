using UnityEngine;
using System.Collections;

public class TrailRunner : MonoBehaviour {

	public Transform pivot;

	public void Move(Vector3 p1, Vector3 p2, float degrees) {

		pivot.position = p1.normalized * GlobeSize.instance.radius;

		Vector3 normal = Vector3.Cross(p1, p2);

		transform.Rotate(normal, degrees, Space.World);

		pivot.LookAt(Vector3.zero, p1);

	}
}
