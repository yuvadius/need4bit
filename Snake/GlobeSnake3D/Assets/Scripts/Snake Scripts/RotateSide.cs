using UnityEngine;
using System.Collections;

public class RotateSide : MonoBehaviour
{

    public Transform lever;
    public CameraRotate cameraRotation;
    public TiltSide tiltSide;
    float speed;

    float prevSpeed;

    float turnAngle = 0;

    InputDriver inputDriver;

    void Start()
    {
        prevSpeed = speed;
        cameraRotation.send_speed(speed);
        inputDriver = FindObjectOfType<InputDriver>();
    }

    // Update is called once per frame
    public void myUpdate()
    {
        if (speed != prevSpeed)
        {
            change_speed();
        }
        float turn = inputDriver.horizontalMove;
        turnAngle = -speed * Time.deltaTime * turn;
        transform.Rotate(lever.localPosition, turnAngle);
        cameraRotation.send_angle(turnAngle);
    }

    void change_speed()
    {
        prevSpeed = speed;
        cameraRotation.send_speed(speed);
    }

    public float get_angle()
    {
        return turnAngle;
    }

    public void set_turn_speed(float turnSpeed)
    {
        speed = turnSpeed;
    }
}
