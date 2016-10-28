using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalManager : MonoBehaviour {

    static Dictionary<string, int> intDictionary = new Dictionary<string,int>();
    static int intDefault = 0;
    static public void SetInt(string name, int number)
    {
        intDictionary.Add(name, number);
    }
    static public int GetInt(string name)
    {
        int result = 0;
        if (intDictionary.TryGetValue(name, out result))
        {
            return result;
        }
        return intDefault;
    }

    static Dictionary<string, float> floatDictionary = new Dictionary<string,float>();
    static float floatDefault = 0;
    static public void SetFloat(string name, float number)
    {
        floatDictionary.Add(name, number);
    }
    static public float GetFloat(string name)
    {
        float result = 0;
        if (floatDictionary.TryGetValue(name, out result))
        {
            return result;
        }
        return floatDefault;
    }
}
