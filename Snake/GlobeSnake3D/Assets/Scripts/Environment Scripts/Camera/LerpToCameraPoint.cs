using UnityEngine;
using System.Collections;

public class LerpToCameraPoint : MonoBehaviour
{

    public Transform lookAtPoint;
    public Transform lerpToPoint;
    public Transform upVector;

    public float lookAtLerpCoefficient = 2f;
    float speed;

    public void myUpdate()
    {
        //Vector3 destination = Vector3.Lerp(lerpToPoint.position, lookAtPoint.position, 0);
        Vector3 newPosition = Vector3.Lerp(transform.position, lerpToPoint.position, speed * Time.deltaTime);

        transform.position = newPosition;
        Quaternion prevRot = transform.rotation;
        transform.LookAt(lookAtPoint, upVector.position);
        Quaternion newRot = transform.rotation;
        transform.rotation = Quaternion.Lerp(prevRot, newRot, lookAtLerpCoefficient * Time.deltaTime);
    }

    public void set_lerp_speed(float lerpSpeed)
    {
        speed = lerpSpeed;
    }
}
