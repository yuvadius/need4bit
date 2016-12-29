using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour 
{
    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
