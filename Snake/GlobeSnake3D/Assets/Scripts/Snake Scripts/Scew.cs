using UnityEngine;
using System.Collections;

public class Scew : MonoBehaviour {

    public Flying fly;
    public RotateForward move;


    float angle = 0;

	public void myUpdate () {
        float forward = move.get_progress();
        float up = fly.get_progress();


        if (forward == 0 || up == 0)
        {
            angle = 0;
        }
        else
        {
            angle = ((Mathf.Atan(up / forward)) * 180f) / Mathf.PI;
        }

	}

    public float get_angle()
    {
        return -angle;
    }


}
