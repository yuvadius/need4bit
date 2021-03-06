﻿using UnityEngine;
using System.Collections;

public class RotationDeviceEmulator : MonoBehaviour {

	public Transform snake;
	public Transform myPivot;
	public Extrapolater extrapo;
	bool set = false;
	public float extrapolationOffset = 0;
	public float emulationOffset = 0;
	public float currentDelta = 0;
	public int avarageConsistancy;
	public float lerpCoefficient = 1;

	float degsPerSec;
	Quaternion nextRot;
	Vector3 extrapPoint;

	bool newExtrap = false;
	float savedTime = 0;

	void Awake() {
		//What is this?
		myPivot.position = myPivot.position.normalized * GlobeSize.instance.radius;
		myPivot.gameObject.SetActive(false);
		GlobeSize.instance.radiusChangedAction += radiusChangedAction;
    }

	void OnDestroy() {
		GlobeSize.instance.radiusChangedAction -= radiusChangedAction;
    }

	public bool myUpdate() {
		if(set == true) {
			if(newExtrap == false) {
				nextRot = extrapo.RotateForward(degsPerSec, 1f);
			} else {
				float framesPassed = (Time.time - savedTime) / Time.fixedDeltaTime;
				nextRot = extrapo.RotateForward(degsPerSec, framesPassed);
			}

			UpdateBy(Time.fixedDeltaTime);

			newExtrap = false;			
		}
		return set;
	}

	void UpdateBy(float time) {
		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			nextRot,
			lerpCoefficient * time
		);

		//snake.LookAt(myPivot.position, myPivot.up); //for anti gliding
		snake.position = myPivot.position;
		snake.rotation = myPivot.rotation;
		currentDelta = Vector3.Angle(snake.position, extrapo.pivot.position);
		emulationOffset = (emulationOffset * avarageConsistancy + currentDelta) / (avarageConsistancy + 1);
		extrapolationOffset = (extrapolationOffset * avarageConsistancy + (myPivot.position - extrapPoint).magnitude) / (avarageConsistancy + 1);
	}

	public void Emulate(Quaternion emulationRotation, Vector3 extrapPoint, Vector3 emulationPoint, float degsPerSec) {
		this.degsPerSec = degsPerSec;
		savedTime = Time.time;

		if(set == false) {
			myPivot.gameObject.SetActive(true);
			transform.rotation = emulationRotation;
			set = true;
		} else {
			nextRot = emulationRotation;
			this.extrapPoint = extrapPoint;
			newExtrap = true;
		}
	}

	void radiusChangedAction(float radius) {
		myPivot.position = myPivot.position.normalized * radius;
	}
}