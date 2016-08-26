using UnityEngine;
using System.Collections;

public class RemoteFowardRotate : MonoBehaviour {

	public Transform forwardLever, sideLever;
	public float turnSpeed, moveSpeed;

	public void SetSpeed(float turnSpeed, float moveSpeed) {
		this.turnSpeed = turnSpeed;
		this.moveSpeed = moveSpeed;
	}

	/// <summary>
	/// This function needs to be called each time that the remote snake changes position, 
	/// 
	/// This will make sure that the position is then made into rotation instructions.
	/// </summary>
	public void ManageDestination(Vector3 Destination) {

	}


	/// <summary>
	/// Rotates the snake by degrees parameter towards the destination
	/// 
	/// This must be called before the rotate forward
	/// </summary>
	private void RotateSide(Vector3 Destination) {

	}

	/// <summary>
	/// Moves the remote snake forward by distance on the globe
	/// 
	/// This must be called after we set a direction
	/// </summary>
	private void RotateFoward() {
		//TODO: rotate forward by distance on the globe
	}

}
