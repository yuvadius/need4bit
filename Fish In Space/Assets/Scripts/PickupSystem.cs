using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupSystem : MonoBehaviour {
    public Transform boundaries;
    public int maxPickups;
    public float pickupRate;//Pickup spawn per second
    private float time;

    void Start()
    {
        time = Time.time;
    }
    
    void Update()
    {
        if (PhotonNetwork.isMasterClient && transform.childCount < maxPickups && Time.time >= time + pickupRate)
        {
            time = Time.time;
            PhotonNetwork.InstantiateSceneObject("Pickup", GetRandomPickupPos(), Quaternion.identity, 0, null);
        }
        
    }

    Vector3 GetRandomPickupPos()
    {
        return new Vector3(Random.Range(-boundaries.localScale.x / 2, boundaries.localScale.x / 2), Random.Range(-boundaries.localScale.y / 2, boundaries.localScale.y / 2), 0);
    }
}
