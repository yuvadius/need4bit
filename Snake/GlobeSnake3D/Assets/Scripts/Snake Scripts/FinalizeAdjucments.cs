using UnityEngine;
using System.Collections;

/// <summary>
/// this script will take the scew and tilt angles and make sure that the rotation is affected by them.
/// the reason that this is done by a third party is because when you tilt then scew then you get the gimble lock
/// effect or something.
/// </summary>
public class FinalizeAdjucments : MonoBehaviour
{

    public TiltSide tilter;
    public Scew scewer;

    public Transform tiltLever;
    public Transform scewLever;

    // Update is called once per frame
    public void myUpdate()
    {
        float tiltAngle = tilter.get_angle();
        float scewAngle = scewer.get_angle();

        transform.localRotation = Quaternion.identity;

        if (tiltAngle != 0 || scewAngle != 0)
        {
            transform.Rotate(tiltLever.localPosition, tiltAngle);
            transform.Rotate(scewLever.localPosition, scewAngle);
        }
    }
}
