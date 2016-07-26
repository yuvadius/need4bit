using UnityEngine;
using System.Collections;

public class AttachToPivot : MonoBehaviour
{

    public Transform pivot;

    // Update is called once per frame
    public void myUpdate()
    {
		transform.position = pivot.position;
		transform.rotation = pivot.rotation;

    }
}
