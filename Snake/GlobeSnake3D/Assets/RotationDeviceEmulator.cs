using UnityEngine;
using System.Collections;

public class RotationDeviceEmulator : MonoBehaviour
{
    public Transform snake;
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

    void Awake()
    {
        //What is this?
        //myPivot.position = myPivot.position.normalized * GameObject.FindGameObjectWithTag("GameController").GetComponent<SnakeController>().tiltDevice.transform.position.magnitude;
        myPivot.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (set == true)
        {
            transform.rotation = Quaternion.Lerp(
				transform.rotation, 
				nextRot, 
				lerpCoefficient * Time.fixedDeltaTime
			);

            snake.LookAt(myPivot.position, myPivot.up);
            snake.position = myPivot.position;
			currentDelta = Quaternion.Angle(transform.rotation, nextRot);
            //currentDelta = (myPivot.position - emulationPoint).magnitude;
            emulationOffset = (emulationOffset * avarageConsistancy + currentDelta) / (avarageConsistancy + 1);
            extrapolationOffset = (extrapolationOffset * avarageConsistancy + (myPivot.position - extrapPoint).magnitude) / (avarageConsistancy + 1);
        }
    }

    public void Emulate(Quaternion emulationRotation, Vector3 extrapPoint, Vector3 emulationPoint)
    {
        if (set == false)
        {
            myPivot.gameObject.SetActive(true);
            transform.rotation = emulationRotation;
            set = true;
        }
        else
        {
            nextRot = emulationRotation;
            this.extrapPoint = extrapPoint;
            this.emulationPoint = emulationPoint;
        }
    }
}