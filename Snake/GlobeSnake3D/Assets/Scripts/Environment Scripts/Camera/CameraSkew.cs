using UnityEngine;
using System.Collections;

public class CameraSkew : MonoBehaviour
{
    public Transform lever;
    public Flying flyHeight;

    float skewAngleAtGlobeLevel;
    float skewAngleAtMaxLevel;

    public void myUpdate()
    {
        float currentHeight = flyHeight.get_current_height();
        float maxHeight = flyHeight.get_max_height();

        float skewAngle = currentHeight * (skewAngleAtMaxLevel - skewAngleAtGlobeLevel) / maxHeight + skewAngleAtGlobeLevel;

        skew(skewAngle);
    }

    void skew(float angle)
    {
        transform.localRotation = Quaternion.identity;
        transform.Rotate(lever.localPosition, -angle);
    }

    public void set_skew_angle_at_global_level(float angle)
    {
        skewAngleAtGlobeLevel = angle;
    }

    public void set_skew_angle_at_max_level(float angle)
    {
        skewAngleAtMaxLevel = angle;
    }

}
