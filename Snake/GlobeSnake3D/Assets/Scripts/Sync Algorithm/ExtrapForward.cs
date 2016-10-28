using UnityEngine;
using System.Collections;

public class ExtrapForward : MonoBehaviour
{
    public Transform lever;
    public float degsPerSec;

    public void myUpdate(float frames)
    {
        float angle = degsPerSec * Time.fixedDeltaTime * frames;
        transform.Rotate(lever.localPosition, angle);
    }
}