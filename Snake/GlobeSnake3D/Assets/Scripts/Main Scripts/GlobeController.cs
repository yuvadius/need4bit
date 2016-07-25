using UnityEngine;
using System.Collections;

public class GlobeController : MonoBehaviour {

    ControlSize globeDevice;
    GlobeGravity gravityDevice;
    AppleController appleController;
    public float globeRadius; float prev1;
    public float globeIncrementalIncrease = 3;
    [Tooltip("The gravity")]
    public float globeAcceleration; float prev2;

    void Start() {
        globeDevice = FindObjectOfType<ControlSize>();
        gravityDevice = FindObjectOfType<GlobeGravity>();
        appleController = FindObjectOfType<AppleController>();

        float globeSizeIncrease = 5;

        prev1 = globeRadius = globeRadius + calcRadiusByIncrease(globeSizeIncrease);
        prev2 = globeAcceleration;

        globeDevice.SetRadius(globeRadius);
        gravityDevice.set_globe_acceleration(globeAcceleration);
    }

    public void myUpdate() {
        if (globeRadius != prev1) {
            prev1 = globeRadius;
            globeDevice.SetRadius(globeRadius);
            appleController.setHeight(globeRadius);

        }

        if (globeAcceleration != prev2) {
            prev2 = globeAcceleration;
            gravityDevice.set_globe_acceleration(globeAcceleration);
        }
    }

    float calcRadiusByIncrease(float increase) {
        return (Mathf.Sqrt(4*globeRadius*globeRadius + increase/Mathf.PI) - 2*globeRadius)/2;
    }

    public void IncreaseGlobeRadius() {
        globeRadius += calcRadiusByIncrease(globeIncrementalIncrease);
    }

}
