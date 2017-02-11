using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    public Transform boundaries;
    public int maxEnemies;
    public float enemyRate;//Enemy spawn per second
    private float time;

    void Start()
    {
        time = Time.time;
    }
    
    void Update()
    {
        if (PhotonNetwork.isMasterClient && transform.childCount < maxEnemies && Time.time >= time + enemyRate)
        {
            time = Time.time;
            PhotonNetwork.InstantiateSceneObject("Enemies/Follow", GetRandomPos(), Quaternion.identity, 0, null);
        }
        
    }

    Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(-boundaries.localScale.x / 2, boundaries.localScale.x / 2), Random.Range(-boundaries.localScale.y / 2, boundaries.localScale.y / 2), 0);
    }
}
