using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PlaceBarrels : MonoBehaviour {
    public int numBarrels;
    private const float delta = 3.575f;
    private static System.Random rnd = new System.Random();

    // Use this for initialization
    void Start () {
        RandomBarrels(numBarrels);
    }

    /// <summary>
    /// This function returns all the spaces that are populatable by
    /// barrels on the map
    /// </summary>
    /// <returns>List of Vector3 of spaces</returns>
    private List<Vector3> GetBarrelSpaces()
    {
        int[] check1 = { -2, 0, 2 };
        int[] check2 = { -3, -2, 2, 3 };
        List<Vector3> spaces = new List<Vector3>();
        for (int i = -3; i < 4; i++)
        {
            for (int j = -3; j < 4; j++)
            {
                if (!(check1.Contains(i) && check1.Contains(j)) && !(check2.Contains(i) && check2.Contains(j)))
                    spaces.Add(new Vector3(i, j) * delta);
            }
        }
        return spaces;
    }

    /// <summary>
    /// Populates the map with barrels
    /// </summary>
    /// <param name="numBarrels">The number of barrels to instantiate</param>
    private void RandomBarrels (int numBarrels)
    {
        List<Vector3> spaces = GetBarrelSpaces();
        for (int i = 0; i < numBarrels; i++)
        {
            int r = rnd.Next(spaces.Count);
            Instantiate(Resources.Load("Barrel"), spaces[r], new Quaternion());
            spaces.RemoveAt(r);
        }
    }
}
