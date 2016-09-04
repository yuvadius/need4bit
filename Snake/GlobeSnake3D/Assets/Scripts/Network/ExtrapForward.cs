using UnityEngine;
using System.Collections;

public class ExtrapForward : MonoBehaviour {

	public Transform lever;

	float height;
	public float speed;
	public float currentSpeed;
	public float degsPerSec;
	public float maxHeight, bonusSpeed;
	public float currentAngle;

	public void SetHeight(float height) {
		this.height = height;
	}

	public void myUpdate() {
		float bonus = calcBonus(height);
		degsPerSec = (currentSpeed + bonus) * 360.0f / (2.0f * Mathf.PI * height);
		float angle = degsPerSec * Time.fixedDeltaTime;
		transform.Rotate(lever.localPosition, angle);
		currentAngle = currentSpeed * Time.fixedDeltaTime;
	}

	float calcBonus(float height) {
		return (bonusSpeed) * height * 0.02f / maxHeight;
	}

}
