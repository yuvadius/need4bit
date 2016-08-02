using UnityEngine;
using System.Collections;

public class SnakeInstance : MonoBehaviour {
    public static SnakeInstance instance;
	
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
