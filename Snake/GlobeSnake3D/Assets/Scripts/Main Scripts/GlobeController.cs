//using UnityEngine;
//using System.Collections;

//public class GlobeController : MonoBehaviour {

//    GlobeSize globeDevice;

//    public float globeRadius; float prev1;
//    public float globeIncrementalIncrease = 3;

//    void Start() {
//        globeDevice = FindObjectOfType<GlobeSize>();

//        globeDevice.SetRadius(globeRadius);
//    }

//    public void myUpdate() {
//        if (globeRadius != prev1) {
//            prev1 = globeRadius;
//            globeDevice.SetRadius(globeRadius);           
//        }
//    }

//    float calcRadiusByIncrease(float increase) {
//        return (Mathf.Sqrt(4*globeRadius*globeRadius + increase/Mathf.PI) - 2*globeRadius)/2;
//    }

//    public void IncreaseGlobeRadius() {
//        globeRadius += calcRadiusByIncrease(globeIncrementalIncrease);
//    }

//}
