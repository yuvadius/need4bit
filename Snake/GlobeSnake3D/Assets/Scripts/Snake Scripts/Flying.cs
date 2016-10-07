using UnityEngine;
using System.Collections;

public class Flying : MonoBehaviour {
    public static Flying instance;
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public GlobeGravity gravity;

    bool inAir;
    public bool isInAir() { return inAir; }

    float flyingSpeed = 0;
    float myHeight;

    float flyingAcceleration;
    float maxFlyingSpeed;
    float maxFlyingHeight;
    float maxFallingSpeed;

    float flyingDistance = 0;

    void Start() {
        attach_to_globe();
    }

    public void myUpdate() {
        if (GlobeSize.instance.radius > transform.position.magnitude) {
            attach_to_globe();
        }
        inAir = true;


        float upAcceleration = Input.GetKey(KeyCode.Space) && myHeight < maxFlyingHeight ? flyingAcceleration : 0;
        flyingSpeed = Mathf.Clamp(flyingSpeed + upAcceleration * Time.deltaTime - GlobeGravity.instance.globeAcceleration * Time.deltaTime, -Mathf.Abs(maxFallingSpeed), maxFlyingSpeed);
        flyingDistance = flyingSpeed * Time.deltaTime;
        fly(flyingDistance);
    }

    void fly(float _flyingDistance) {
        myHeight += _flyingDistance;
        if (myHeight > 0) {
            set_height(myHeight);
        } else {
            attach_to_globe();
        }
    }

    void set_height(float height) {
        transform.position = transform.position.normalized * (height + GlobeSize.instance.radius);
    }

    void attach_to_globe() {
        inAir = false;
        transform.position = transform.position.normalized * GlobeSize.instance.radius;
        myHeight = 0;
        reset();
    }

    void reset() {
        flyingSpeed = 0;
        flyingDistance = 0;
    }

    public float get_progress() {
        return flyingDistance;
    }
    public void set_flying_acceleration(float _flyingAcceleration) {
        flyingAcceleration = _flyingAcceleration;
    }
    public void set_max_flying_speed(float _maxFlyingSpeed) {
        maxFlyingSpeed = _maxFlyingSpeed;
    }
    public void set_max_flying_height(float _maxFlyingHeight) {
        maxFlyingHeight = _maxFlyingHeight;
    }
    public void set_max_falling_speed(float _maxFallingSpeed) {
        maxFallingSpeed = _maxFallingSpeed;
    }

    public float get_current_height() {
        return myHeight;
    }
    public float get_max_height() {
        return maxFlyingHeight;
    }
}
