using UnityEngine;
using System.Collections;

public class Game_Controller : MonoBehaviour 
{
    private float curr_time;
    public float pop_rate;
    public GameObject pread;

	// Use this for initialization
	void Start () 
    {
        curr_time = 0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        curr_time += Time.deltaTime;

        if (curr_time > pop_rate)
        {
            curr_time = 0f;
            Vector3 pos = new Vector3(Random.Range(-2.4f, 2.4f), Random.Range(-1.1f, 1.1f), 0);
            Quaternion rot = new Quaternion(0f,0f,0f, 0f);
            Instantiate(pread, pos, rot);
            
        }
	}
}
