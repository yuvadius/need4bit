using UnityEngine;
using System.Collections;

public class TiltSide : MonoBehaviour
{

    public float maxTiltAngle = 45;
    public float tiltSpeed = 20;
    public float returnSpeed = 100;
    public float dieLerpSpeed = 4;

    float currentTilt = 0;
    bool isDeadTilt = false;

    InputDriver inputDriver;

    void Start() {
        inputDriver = FindObjectOfType<InputDriver>();
    }


    // Update is called once per frame
    public void myUpdate()
    {
        if (isDeadTilt == false) {
            float addTilt = 0;

            if (inputDriver.horizontalMove > 0) {
                if (currentTilt > 0) {
                    addTilt = tiltSpeed * Time.deltaTime;
                } else {
                    addTilt = (returnSpeed+tiltSpeed) * Time.deltaTime;
                }
                if (currentTilt + addTilt > maxTiltAngle * inputDriver.horizontalMove) {
                    addTilt = maxTiltAngle * inputDriver.horizontalMove - currentTilt;
                    currentTilt = maxTiltAngle * inputDriver.horizontalMove;
                } else {
                    currentTilt += addTilt;
                }
            } else if (inputDriver.horizontalMove < 0) {
                if (currentTilt < 0) {
                    addTilt = -tiltSpeed * Time.deltaTime;
                } else {
                    addTilt = -(returnSpeed + tiltSpeed) * Time.deltaTime;
                }
                if (currentTilt + addTilt < -maxTiltAngle * -inputDriver.horizontalMove) {
                    addTilt = -maxTiltAngle * -inputDriver.horizontalMove - currentTilt;
                    currentTilt = -maxTiltAngle * -inputDriver.horizontalMove;
                } else {
                    currentTilt += addTilt;
                }
            } else {
                if (currentTilt > 0) {
                    addTilt = -returnSpeed * Time.deltaTime;
                    if (currentTilt - addTilt < 0) {
                        addTilt = -currentTilt;
                        currentTilt = 0;
                    } else {
                        currentTilt += addTilt;
                    }
                } else if (currentTilt < 0) {
                    addTilt = returnSpeed * Time.deltaTime;
                    if (currentTilt + addTilt > 0) {
                        addTilt = -currentTilt;
                        currentTilt = 0;
                    } else {
                        currentTilt += addTilt;
                    }
                }
            }
        } else {
            if (currentTilt > 0) {
                currentTilt = Mathf.Lerp(currentTilt, 90, dieLerpSpeed * Time.deltaTime);
            } else {
                currentTilt = Mathf.Lerp(currentTilt, -90, dieLerpSpeed * Time.deltaTime);
            }
        }
    }

    public void DieTilt() {
        isDeadTilt = true;
    }

    public void set_max_tilt_angle(float _maxTiltAngle)
    {
        maxTiltAngle = _maxTiltAngle;
    }
    public void set_tilt_speed(float _tiltSpeed)
    {
        tiltSpeed = _tiltSpeed;
    }
    public void set_tilt_return_speed(float _tiltReturnSpeed)
    {
        returnSpeed = _tiltReturnSpeed; 
    }

    public float get_angle()
    {
        return -currentTilt;
    }
}
