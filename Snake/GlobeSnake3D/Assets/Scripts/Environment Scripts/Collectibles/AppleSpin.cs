using UnityEngine;
using System.Collections;

public class AppleSpin : MonoBehaviour {
  
    public float minSpeed = 30f, maxSpeed = 180;

    float spinSpeed;
    Vector3 lever;

	void Start(){
        lever = new Vector3(
            Random.Range(-1, 1), 
            Random.Range(-1, 1), 
            Random.Range(8, 10)
        );
		lever = lever.normalized;

        spinSpeed = Random.Range(minSpeed, maxSpeed);
	}

    void Update()
    {
        transform.Rotate(lever, spinSpeed * Time.deltaTime);
    }

}
