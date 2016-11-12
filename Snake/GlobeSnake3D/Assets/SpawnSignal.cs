using UnityEngine;
using System.Collections;

public class SpawnSignal : MonoBehaviour {

	public Camera cam;
	public LayerMask layerMask;

	public ParticleSystem system;

	bool isShow = true;

	float stopTime = 5f;

	void Update () {
		if (isShow) {
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 1000f, layerMask)) {
				//print (hit.point);
				transform.LookAt (hit.point);
				transform.Rotate (90, 0, 0);
			}
		}
	}

	public void StopThis(){
		isShow = false;
		StartCoroutine (StopThisCo ());
	}

	IEnumerator StopThisCo(){
		float timePassed = 0;

		while (timePassed < stopTime) {

			system.transform.localScale = (1 - timePassed / stopTime) * Vector3.one;

			timePassed += Time.deltaTime;
			yield return null;
		}

		Destroy (gameObject);
	}

}
