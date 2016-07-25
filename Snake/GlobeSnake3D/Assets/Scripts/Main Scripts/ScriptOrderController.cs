using UnityEngine;
using System.Collections;

public class ScriptOrderController : MonoBehaviour {

    CameraController cameraController;
    RotateForward rotateForward;
    RotateSide rotateSide;
    Scew scew;
    Trail trail;
    SnakeController snakeController;
    TiltSide tiltSide;
    AttachToPivot attachToPivot;
    GlobeController globeController;
	AppleController appleController;
	BannanaController bannanaController;
    LerpToCameraPoint lerpToCameraPoint;
    FinalizeAdjucments finilizeAdjucments;
    Flying flyingDevice;
    CameraHeight cameraHeight;
    CameraSkew cameraSkew;
	CapacityController capacityController;

    bool haveStarted = false;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
		appleController = FindObjectOfType<AppleController>();
        rotateForward = FindObjectOfType<RotateForward>();
        rotateSide = FindObjectOfType<RotateSide>();
        scew = FindObjectOfType<Scew>();
        trail = FindObjectOfType<Trail>();
        snakeController = FindObjectOfType<SnakeController>();
        tiltSide = FindObjectOfType<TiltSide>();
        attachToPivot = FindObjectOfType<AttachToPivot>();
        globeController = FindObjectOfType<GlobeController>();
        lerpToCameraPoint = FindObjectOfType<LerpToCameraPoint>();
        finilizeAdjucments = FindObjectOfType<FinalizeAdjucments>();
        flyingDevice = FindObjectOfType<Flying>();
        cameraHeight = FindObjectOfType<CameraHeight>();
        cameraSkew = FindObjectOfType<CameraSkew>();
		bannanaController = FindObjectOfType<BannanaController>();
 
    }

    /// <summary>
    /// the order might be important, any known dependencies document here
    /// 
    /// proper skew requires to know this frames change in height and forward, so the scew must come after
    /// rotateForward and flyingDevice
    /// 
    /// for the changes in the controllers to take effect during this frame, they need to be called first.
    /// 
    /// trail should be called after the final resting place of this frame has been set, after all tilts and scews and rotates
    /// 
    /// attach to pivot should also be called after the final resting place has been set.
    /// 
    /// camera lerping should be called also after the final resting place of this frame
    /// 
    /// the finilizyAdjucments must have this frames lates tilt and scew angles, that why it must be called after TiltSide and scew.
    /// 
    /// CameraSkew script needs to be set before lerp to camera point, and after the flying has set its height.
    /// 
    /// the CameraHeight script is depended on the flying height, so it would be wise to place it after Flying script
    ///
	/// the AppleController responsible for the apple hights, needs the current radius, thus
	/// must should be called after the globeController.myUpdate()
	/// 
	/// bannana controller can be called whenever because it is not depended on anything almost
	///
	/// capacity controller needs to know the current state of the entire trail, thus it should be 
	/// after it.
	/// </summary>
    void FixedUpdate()
    {
        if (haveStarted == true) {
            cameraController.myUpdate();
            snakeController.myUpdate();
            globeController.myUpdate();
            appleController.myUpdate();
            bannanaController.myUpdate();

            rotateForward.myUpdate();
            rotateSide.myUpdate();

            flyingDevice.myUpdate();

            cameraSkew.myUpdate();

            cameraHeight.myUpdate();

            tiltSide.myUpdate();

            scew.myUpdate();

            finilizeAdjucments.myUpdate();

            attachToPivot.myUpdate();

            trail.myUpdate();

            lerpToCameraPoint.myUpdate();
        }
    }

    public void StartGame() {
        haveStarted = true;
    }
    
}
