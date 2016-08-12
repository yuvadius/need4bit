using UnityEngine;
using System.Collections;

public class LocalSnake : MonoBehaviour {
    public static LocalSnake instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
