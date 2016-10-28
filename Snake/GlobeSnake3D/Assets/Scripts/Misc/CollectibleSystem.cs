using UnityEngine;
using System.Collections;

public class CollectibleSystem : MonoBehaviour {
    private static CollectibleSystem instance;

    void Awake()
    {


        if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
}
