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
	Vector3 pathPoint;

	public GameObject extrapGizmo, pathGizmo, emulationGizmo, finalGizmo;
	public Transform gizmoHolder;
	public EmulationTrail trail;

	public bool drawGizmos = false;

	void Awake() {
		myPivot.position = myPivot.position.normalized * GameObject.FindGameObjectWithTag("GameController").GetComponent<SnakeController>().tiltDevice.transform.position.magnitude;
		extrapGizmo = FindObjectOfType<MainController>().extrapPointGizmo;
		pathGizmo = FindObjectOfType<MainController>().pathPointGizmo;
		emulationGizmo = FindObjectOfType<MainController>().emulationPointGizmo;
		finalGizmo = FindObjectOfType<MainController>().finalPointGizmo;
		gizmoHolder = FindObjectOfType<MainController>().gizmoHolder;
		myPivot.gameObject.SetActive(false);
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

			if(drawGizmos)
				CreateNewGizmoSet();

			trail.myUpdate();
        }

	}

	void CreateNewGizmoSet() {
		GameObject newHolder = new GameObject();
		newHolder.transform.SetParent(gizmoHolder);
		newHolder.name = "Gizmo: " + Time.frameCount;

		GameObject newGizmo = Instantiate(extrapGizmo, extrapPoint, Quaternion.identity) as GameObject;
		newGizmo.SetActive(true);
		newGizmo.transform.SetParent(newHolder.transform);
		newGizmo.name = "Extrap Point";

		GameObject newGizmo2 = Instantiate(finalGizmo, myPivot.position, Quaternion.identity) as GameObject;
		newGizmo2.SetActive(true);
		newGizmo2.transform.SetParent(newHolder.transform);
		newGizmo2.name = "Final Point";

		GameObject newGizmo3 = Instantiate(pathGizmo, pathPoint, Quaternion.identity) as GameObject;
		newGizmo3.SetActive(true);
		newGizmo3.transform.SetParent(newHolder.transform);
		newGizmo3.name = "Path Point";

		GameObject newGizmo4 = Instantiate(emulationGizmo, emulationPoint, Quaternion.identity) as GameObject;
		newGizmo4.SetActive(true);
		newGizmo4.transform.SetParent(newHolder.transform);
		newGizmo4.name = "Emulation Point";
	}

	public void Emulate(Quaternion emulationRotation, Vector3 pathPoint, Vector3 extrapPoint, Vector3 emulationPoint) {
		if( set == false) {
			myPivot.gameObject.SetActive(true);
			transform.rotation = emulationRotation;
			set = true;
		} else {
			nextRot = emulationRotation;
			this.pathPoint = pathPoint;
			this.extrapPoint = extrapPoint;
			this.emulationPoint = emulationPoint;
        }
	}

}
