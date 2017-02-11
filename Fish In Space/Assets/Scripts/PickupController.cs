using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {
    public int score;

    void Start()
    {
        transform.parent = GameObject.Find("Pickups").transform;
    }

    public float GetVolume()
    {
        return Matchmaker.ScoreToVolume(score);
    }

    public int GetScore()
    {
        return score;
    }
}
