using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

    public Transform lever;

    //this is how much the camera needs to rotate in order to be behind the snake
    float feedback = 0;
    float maxFeedback = 90;

    float turnSpeed = 0;
    float maxTurnSpeedToRotate = 1;
    float snakeTurnSpeed;
    float turnAccelarationAtZero;
    float turnAccelarationAtMax;

    //we manually call this function every time that a new angle is sent.
    void myUpdate(){
        float turnAccelaration;
        if (maxFeedback == 0)
            turnAccelaration = turnAccelarationAtMax;
        else 
            turnAccelaration = turnAccelarationAtZero + Mathf.Abs(feedback) * (turnAccelarationAtMax - turnAccelarationAtZero) / maxFeedback ;
        if (feedback < 0)
        {
            turnSpeed -= turnAccelaration * Time.deltaTime;
        }
        else if (feedback > 0)
        {
            turnSpeed += turnAccelaration * Time.deltaTime;
        }

        if (turnSpeed > snakeTurnSpeed * maxTurnSpeedToRotate)
        {
            turnSpeed = snakeTurnSpeed * maxTurnSpeedToRotate;
        }
        else if (turnSpeed < -snakeTurnSpeed * maxTurnSpeedToRotate)
        {
            turnSpeed = -snakeTurnSpeed * maxTurnSpeedToRotate;
        }
        
        if (feedback != 0)
        {
            float turnAngle = turnSpeed * Time.deltaTime;
            rotate_camera(turnAngle);
        }

    }

    void rotate_camera(float angle)
    {
        float newFeedback = feedback - angle;
        if (newFeedback > maxFeedback)
        {

            rotate(feedback - maxFeedback);
            feedback = maxFeedback;
            turnSpeed = 0;

        }
        else if (newFeedback < -maxFeedback)
        {
            rotate(feedback + maxFeedback);
            feedback = -maxFeedback;
            turnSpeed = 0;
        }
        else if (feedback < 0 && newFeedback > 0)
        {
            rotate(feedback);
            feedback = 0;
            turnSpeed = 0;

        }
        else if (feedback > 0 && newFeedback < 0)
        {
            rotate(feedback);
            feedback = 0;
            turnSpeed = 0;
        }
        else
        {
            rotate(angle);
            feedback -= angle;
        }

    }

    void rotate(float angle)
    {
        transform.Rotate(lever.localPosition, angle);
    }

    //each time that CameraRotate recieves this function call, it needs to adjust
    public void send_angle(float angle)
    {
        feedback -= angle;
        rotate(angle);
        myUpdate();
    }

    public void send_speed(float _speed)
    {
        snakeTurnSpeed = _speed;
    }

    public void set_max_camera_layback(float maxCameraLayback){
        maxFeedback = maxCameraLayback;
    }
    public void set_max_layback_turn_speed(float maxLaybackTurnSpeed)
    {
        maxTurnSpeedToRotate = maxLaybackTurnSpeed;
    }
    public void set_turn_accelaration_zero(float turnAccelarationZero)
    {
        turnAccelarationAtZero = turnAccelarationZero;
    }
    public void set_turn_accelaration_max(float turnAccelarationMax)
    {
        turnAccelarationAtMax = turnAccelarationMax;
    }
 
}
