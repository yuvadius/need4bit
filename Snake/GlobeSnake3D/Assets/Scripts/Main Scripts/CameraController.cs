using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    LerpToCameraPoint cameraFollowDevice;
    CameraRotate cameraDevice;
    CameraHeight cameraHeightDevice;
    CameraSkew cameraSkewDevice;

    public float lerpSpeed;                 float prev1;
    public float maxCameraLayback;          float prev2;
    public float maxLaybackTurnSpeed;       float prev3;
    public float turnAccelarationZero;      float prev4;
    public float turnAccelarationMax;       float prev5;
    public float cameraHeightAtGlobeLevel;  float prev6;
    public float cameraHeightAtMaxLevel;    float prev7;
    public float skewAngleAtGlobeLevel;     float prev8;
    public float skewAngleAtMaxLevel;       float prev9;

    void Start()
    {
        Cursor.visible = true;  

        cameraFollowDevice = FindObjectOfType<LerpToCameraPoint>();
        cameraDevice = FindObjectOfType<CameraRotate>();
        cameraHeightDevice = FindObjectOfType<CameraHeight>();
        cameraSkewDevice = FindObjectOfType<CameraSkew>();

        prev1 = lerpSpeed;
        prev2 = maxCameraLayback;
        prev3 = maxLaybackTurnSpeed;
        prev4 = turnAccelarationZero;
        prev5 = turnAccelarationMax;
        prev6 = cameraHeightAtGlobeLevel;
        prev7 = cameraHeightAtMaxLevel;
        prev8 = skewAngleAtGlobeLevel;
        prev9 = skewAngleAtMaxLevel;

        cameraHeightDevice.set_height_at_globe_level(cameraHeightAtGlobeLevel);
        cameraHeightDevice.set_height_at_max_level(cameraHeightAtMaxLevel);
        cameraFollowDevice.set_lerp_speed(lerpSpeed);
        cameraDevice.set_max_camera_layback(maxCameraLayback);
        cameraDevice.set_max_layback_turn_speed(maxLaybackTurnSpeed);
        cameraDevice.set_turn_accelaration_zero(turnAccelarationZero);
        cameraDevice.set_turn_accelaration_max(turnAccelarationMax);
        cameraSkewDevice.set_skew_angle_at_global_level(skewAngleAtGlobeLevel);
        cameraSkewDevice.set_skew_angle_at_max_level(skewAngleAtMaxLevel);
    }

    public void myUpdate()
    {
        if (lerpSpeed != prev1)
        {
            prev1 = lerpSpeed;
            cameraFollowDevice.set_lerp_speed(lerpSpeed);
        }

        if (maxCameraLayback != prev2)
        {
            prev2 = maxCameraLayback;
            cameraDevice.set_max_camera_layback(maxCameraLayback);
        }

        if (maxLaybackTurnSpeed != prev3)
        {
            prev3 = maxLaybackTurnSpeed;
            cameraDevice.set_max_layback_turn_speed(maxLaybackTurnSpeed);
        }

        if (turnAccelarationZero != prev4)
        {
            prev4 = turnAccelarationZero;
            cameraDevice.set_turn_accelaration_zero(turnAccelarationZero);
        }

        if (turnAccelarationMax != prev5)
        {
            prev5 = turnAccelarationMax;
            cameraDevice.set_turn_accelaration_max(turnAccelarationMax);
        }

        if (cameraHeightAtGlobeLevel != prev6)
        {
            prev6 = cameraHeightAtGlobeLevel;
            cameraHeightDevice.set_height_at_globe_level(cameraHeightAtGlobeLevel);
        }

        if (cameraHeightAtMaxLevel != prev7)
        {
            prev7 = cameraHeightAtMaxLevel;
            cameraHeightDevice.set_height_at_max_level(cameraHeightAtMaxLevel);
        }

        if (skewAngleAtGlobeLevel != prev8)
        {
            prev8 = skewAngleAtGlobeLevel;
            cameraSkewDevice.set_skew_angle_at_global_level(skewAngleAtGlobeLevel);
        }

        if (skewAngleAtMaxLevel != prev9)
        {
            prev9 = skewAngleAtMaxLevel;
            cameraSkewDevice.set_skew_angle_at_max_level(skewAngleAtMaxLevel);
        }
    }

}
