using UnityEngine;
using System.Collections;

public class CameraAspect : MonoBehaviour {

    public float aspect = 1.0f;

    void Awake()
    {
        GetComponent<Camera>().aspect = aspect;
        float newCamX = GetComponent<Camera>().aspect * GetComponent<Camera>().orthographicSize;
        float newCamY = GetComponent<Camera>().orthographicSize;
        transform.position = new Vector3(newCamX, newCamY, -10);
    }

    public float GetAspect() { return aspect; }
    public float GetSize() { return GetComponent<Camera>().orthographicSize; }
    public float GetWidth() { return aspect * GetComponent<Camera>().orthographicSize * 2; }
    public float GetHeight() { return GetComponent<Camera>().orthographicSize * 2; }
    public float GetXPos() { return transform.position.x; }
    public float GetYPos() { return transform.position.y; }
    public float GetViewportX() { return GetComponent<Camera>().rect.x; }
    public float GetViewportY() { return GetComponent<Camera>().rect.y; }
    public float GetViewportWidth() { return GetComponent<Camera>().rect.width; }
    public float GetViewportHeight() { return GetComponent<Camera>().rect.height; }

}
