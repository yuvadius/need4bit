using UnityEngine;
using System.Collections;

public class RotationDeviceEmulator : MonoBehaviour {

	public Transform myPivot;

	bool set = false;

	public float extrapolationOffset = 0;
	public float emulationOffset = 0;
	public float currentDelta = 0;
	public int avarageConsistancy;
	public float lerpCoefficient = 1;

	Quaternion nextRot;
	Vector3 extrapPoint;
	Vector3 emulationPoint;

	public GameObject extrapGizmo, pathGizmo;

	void Awake() {
		myPivot.position = myPivot.position.normalized * GameObject.FindGameObjectWithTag("GameController").GetComponent<SnakeController>().tiltDevice.transform.position.magnitude;
		extrapGizmo = FindObjectOfType<MainController>().extrapPointGizmo;
		pathGizmo = FindObjectOfType<MainController>().pathPointGizmo;
    }

	void FixedUpdate() {

		if( set == true) {

			transform.rotation = Quaternion.Lerp(
				transform.rotation, 
				nextRot, 
				lerpCoefficient * Time.fixedDeltaTime
			);

			currentDelta = (myPivot.position - emulationPoint).magnitude;
			emulationOffset = (emulationOffset * avarageConsistancy + currentDelta) / (avarageConsistancy + 1);
			extrapolationOffset = (extrapolationOffset * avarageConsistancy + (myPivot.position - extrapPoint).magnitude) / (avarageConsistancy + 1);

			GameObject newGizmo = Instantiate(extrapGizmo, extrapPoint, Quaternion.identity) as GameObject;
			newGizmo.SetActive(true);

			GameObject newGizmo2 = Instantiate(pathGizmo, transform.position, Quaternion.identity) as GameObject;
			newGizmo2.SetActive(true);
		}

	}

	public void SetNextRot(Quaternion rot, Vector3 extrapPoint, Vector3 emulationPoint) {
		if( set == false) {
			transform.rotation = rot;
			set = true;
		} else {
			nextRot = rot;
			this.extrapPoint = extrapPoint;
			this.emulationPoint = emulationPoint;
        }
	}

}
