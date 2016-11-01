using UnityEngine;
using System.Collections;

/// <summary>
/// Rotate forward. This script needs to rotate the forward lever according to 
/// the distance covered by the final speed, which is normal speed + bonus 
/// from height.
/// </summary>
public class RotateForward : MonoBehaviour {

    public Transform lever;
    public Transform pivot;

    bool moveSpeedLock = true;
    float speed;
    public float currentSpeed;
	public float degsPerSec;
	float maxHeight, bonusSpeed;

	// Use this for initialization
	public void ManualStart (float waitTime, float slowTime) {
        StartCoroutine(slow_start(waitTime, slowTime));
	}

    IEnumerator slow_start(float waitTime, float slowTime)
    {
        moveSpeedLock = true;
        float time = 0;
        currentSpeed = 0;

        while (waitTime > 0) {
			waitTime -= Time.deltaTime;
            currentSpeed = 0;

            yield return null;
        }

        while (time < slowTime)
        {
            time += Time.deltaTime;

            currentSpeed = time * speed / slowTime;
            if (currentSpeed > speed)
                currentSpeed = speed;

            yield return null;
        }
        currentSpeed = speed;
        moveSpeedLock = false;
    }

    float currentAngle;
	// Update is called once per frame
	public void myUpdate () {		
        float height = pivot.position.magnitude;
        float bonus = calcBonus(height);
		degsPerSec = (currentSpeed+bonus) * 360.0f / (2.0f * Mathf.PI * height);
		float angle = degsPerSec * Time.deltaTime;
		transform.Rotate(lever.localPosition, angle);
        currentAngle = currentSpeed * Time.deltaTime;
	}

    public bool IsStarted() {
        return moveSpeedLock == false;
    }
	float calcBonus(float height){

		return (bonusSpeed) * height * 0.02f / maxHeight;
	}

	public void setMaxHeightAndBonusSpeed(float maxHeight, float bonusSpeed){
		this.maxHeight = maxHeight;
		this.bonusSpeed = bonusSpeed;
	}

    public void set_move_speed(float _moveSpeed){
        currentSpeed = speed = _moveSpeed;
    }

    public float get_progress()
    {
        return currentAngle;
    }

}
