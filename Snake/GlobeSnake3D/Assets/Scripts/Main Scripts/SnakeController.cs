using UnityEngine;
using System.Collections;

public class SnakeController : MonoBehaviour {

	public static SnakeController instance;


    public float firstSegmentDistance = 0.28f; float prev1;
    public float segmentDistance = 0.19f; float prev2;

    [Tooltip("How much speed there should be")]
    public float constSpeed = 1f;
    [Tooltip("How much speed there actually is right now")]
    public float moveSpeed = 1f; float prev3;
    [Tooltip("how much faster you move when you press up")]
    public float moveExtraUp = 0.15f;
    [Tooltip("how much slower you move when you press down")]
    public float moveExtraDown = 0.15f;
    [Tooltip("max fast extra movement that lvl up can give")]
    public float moveUpLimit = 0.90f;
    [Tooltip("min slow movement that lvl up can give")]
    public float moveDownLimit = 0.30f;
    [Tooltip("by how much to lerp improve each level up")]
    public float lvlUpMoveImprove = 0.02f;
    public float maxTurnSpeed = 180f;
    public float turnSpeedIncrement = 0.02f;
    public float turnSpeed = 150f; float prev4;

    public float flyingAcceleration; float prev8;
    public float maxFlyingSpeed; float prev9;
    public float maxFlyingHeight; float prev10;
    public float maxFallingSpeed; float prev11;

    public float maxTiltAngle = 30f; float prev5;
    public float tiltSpeed = 20f; float prev6;
    public float tiltReturnSpeed = 40f; float prev7;

    public SkinnedMeshRenderer skin;
    public Trail trail;
    public RotateForward moveDevice;
    public RotateSide turnDevice;
    public TiltSide tiltDevice;
    public Flying flyDevice;

    bool lockSpeed = false;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start()
    {
        prev1 = firstSegmentDistance;
        prev2 = segmentDistance;
        prev3 = moveSpeed;
        prev4 = turnSpeed;
        prev5 = maxTiltAngle;
        prev6 = tiltSpeed;
        prev7 = tiltReturnSpeed;
        prev8 = flyingAcceleration;
        prev9 = maxFlyingSpeed;
        prev10 = maxFlyingHeight;
        prev11 = maxFallingSpeed;

        trail.set_first_segment_distance(firstSegmentDistance);
        trail.set_segment_distance(segmentDistance);
        moveDevice.set_move_speed(moveSpeed);
		moveDevice.setMaxHeightAndBonusSpeed(maxFlyingHeight, 10);
        turnDevice.set_turn_speed(turnSpeed);
        tiltDevice.set_max_tilt_angle(maxTiltAngle);
        tiltDevice.set_tilt_speed(tiltSpeed);
        tiltDevice.set_tilt_return_speed(tiltReturnSpeed);
        flyDevice.set_flying_acceleration(flyingAcceleration);
        flyDevice.set_max_flying_speed(maxFlyingSpeed);
        flyDevice.set_max_flying_height(maxFlyingHeight);
        flyDevice.set_max_falling_speed(maxFallingSpeed);
    }

    public void myUpdate()
    {
        if (firstSegmentDistance != prev1)
        {
            prev1 = firstSegmentDistance;
            trail.set_first_segment_distance(firstSegmentDistance);
        }

        if (segmentDistance != prev2)
        {
            prev2 = segmentDistance;
            trail.set_segment_distance(segmentDistance);
        }

        if (moveSpeed != prev3)
        {
            prev3 = moveSpeed;
            moveDevice.set_move_speed(moveSpeed);
        }

        if (turnSpeed != prev4)
        {
            prev4 = turnSpeed;
            turnDevice.set_turn_speed(turnSpeed);
        }

        if (maxTiltAngle != prev5)
        {
            prev5 = maxTiltAngle;
            tiltDevice.set_max_tilt_angle(maxTiltAngle);
        }

        if (tiltSpeed != prev6)
        {
            prev6 = tiltSpeed;
            tiltDevice.set_tilt_speed(tiltSpeed);
        }

        if (tiltReturnSpeed != prev7)
        {
            prev7 = tiltReturnSpeed;
            tiltDevice.set_tilt_return_speed(tiltReturnSpeed);
        }

        if (flyingAcceleration != prev8)
        {
            prev8 = flyingAcceleration;
            flyDevice.set_flying_acceleration(flyingAcceleration);
        }

        if (maxFlyingSpeed != prev9)
        {
            prev9 = maxFlyingSpeed;
            flyDevice.set_max_flying_speed(maxFlyingSpeed);
        }

        if (maxFlyingHeight != prev10)
        {
            prev10 = maxFlyingHeight;
            flyDevice.set_max_flying_height(maxFlyingHeight);
            moveDevice.setMaxHeightAndBonusSpeed(maxFlyingHeight, 0);
        }

        if (maxFallingSpeed != prev11)
        {
            prev11 = maxFallingSpeed;
            flyDevice.set_max_falling_speed(maxFallingSpeed);
        }
    }

    public void IncreaseMovement() {
        moveExtraUp = Mathf.Lerp(moveExtraUp, moveUpLimit, lvlUpMoveImprove);
        moveExtraDown = Mathf.Lerp(moveExtraDown, moveDownLimit, lvlUpMoveImprove);
        moveSpeed += moveUpLimit;
    }

    public void IncrementTurn() {
        turnSpeed = Mathf.Lerp(turnSpeed, maxTurnSpeed, turnSpeedIncrement);
    }

    public void SetNewSpeed(float newSpeed) {
        if (lockSpeed == true) {
            return;
        }
        
        moveSpeed = newSpeed;
    }

	public void stop(float seconds){
        lockSpeed = true;
		StartCoroutine(stopMoving(seconds));
	}

	IEnumerator stopMoving(float seconds){
		float time = seconds;
		float fromSpeed = moveSpeed;
		float fromTurn = turnSpeed;
		flyingAcceleration = 0;

		while( time>0 ){
			moveSpeed = fromSpeed * (time/seconds);
			turnSpeed = fromTurn * (time/seconds);

			time -= Time.deltaTime;
			yield return null;
		}
		moveSpeed = 0;
	}
}







