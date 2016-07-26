using UnityEngine;
using System.Collections;

public class ControlSize : MonoBehaviour
{
    float starterRadius = 0.5f; //this must the size of the globe in the scene metrics. by radius.
    float radius;

    public void SetRadius(float radius)
    {
        this.radius = radius;
        scale();
    }

    private void scale()
    {
        float newScale = radius / starterRadius;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
