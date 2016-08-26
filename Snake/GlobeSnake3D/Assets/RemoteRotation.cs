using UnityEngine;
using System.Collections;

public class RemoteRotation : Photon.MonoBehaviour
{
    public Transform snake;
    public Transform remotePivot;
    private GameObject rotationDevice;

	void Start () {
        rotationDevice = GameObject.FindGameObjectWithTag("RotationDevice");
	}
	
	void Update () {
        if (photonView.isMine)
            transform.rotation = rotationDevice.transform.rotation;
        else
            snake.position = remotePivot.position;
	}
}
