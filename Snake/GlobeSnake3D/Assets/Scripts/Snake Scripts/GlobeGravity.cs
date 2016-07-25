using UnityEngine;
using System.Collections;

public class GlobeGravity : MonoBehaviour {


    float gravityAcceleration;

    public void set_globe_acceleration(float _globeAcceleration)
    {
        gravityAcceleration = _globeAcceleration;
    }

    public float get_gravity_accceleration()
    {
        return gravityAcceleration;
    }


}
