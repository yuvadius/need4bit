using UnityEngine;
using System.Collections;

public class RemoteFowardRotate : MonoBehaviour {

	public Transform forwardLever, sideLever;
	public float turnSpeed, moveSpeed;
	public Transform pivot, pivotForward, pivotRight, projectedDestination;

	Vector3 destination = Vector3.one;
    bool hasSet = false;

	public float globeRadius = 0.5f;


	[ContextMenu("Try")]
	void Update() {
        if (hasSet == false)
        {
            return;
        }
		//set up some vectors
		Vector3 myForwardVector = pivotForward.position - pivot.position;
		Vector3 myRightVector = pivotRight.position - pivot.position;
		projectedDestination.position = Vector3.ProjectOnPlane(destination, pivot.position.normalized) + pivot.position;
		Vector3 desVector = projectedDestination.position - pivot.position;

		//set up some dot products
		float dot = Vector3.Dot(myForwardVector.normalized, desVector.normalized);
		float rDot = Vector3.Dot(myRightVector.normalized, desVector.normalized);

		//figure out the angle
		float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
		if( rDot < 0 ) {
			angle *= -1f;
		}

		RotateSide(angle);
		RotateFoward();
    }


	public void SetSpeed(float turnSpeed, float moveSpeed) {
		this.turnSpeed = turnSpeed;
		this.moveSpeed = moveSpeed;
	}

	/// <summary>
	/// This function needs to be called each time that the remote snake changes position, 
	/// 
	/// This will make sure that the position is then made into rotation instructions.
	/// </summary>
	public void ManageDestination(Vector3 destination) {
        this.destination = destination;
        hasSet = true;
	}

	/// <summary>
	/// Rotates the snake by degrees parameter towards the destination
	/// 
	/// This must be called before the rotate forward
	/// </summary>
	private void RotateSide(float angle) {
		float turn = angle > 0 ? 1 : -1;
		float turnAngle = turnSpeed * Time.deltaTime * turn;
		turnAngle = Mathf.Clamp(turnAngle, -Mathf.Abs(angle), Mathf.Abs(angle));
		transform.Rotate(sideLever.localPosition, turnAngle);
	}

	/// <summary>
	/// Moves the remote snake forward by distance on the globe
	/// 
	/// This must be called after we set a direction
	/// </summary>
	private void RotateFoward() {
		float dot = Vector3.Dot(
			pivot.position,
			destination
		);
		float reqAangle = Mathf.Acos(dot) * Mathf.Rad2Deg;

		float distanceThisFrame = moveSpeed * Time.deltaTime;
		float circumference = 2*Mathf.PI*globeRadius;
		float angleThisFrame = distanceThisFrame*360f/circumference;

		angleThisFrame = Mathf.Clamp(angleThisFrame, 0, reqAangle);
		transform.Rotate(forwardLever.localPosition, angleThisFrame);
	}

}
