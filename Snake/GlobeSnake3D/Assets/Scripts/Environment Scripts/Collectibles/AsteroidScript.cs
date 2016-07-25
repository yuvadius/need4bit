using UnityEngine;
using System.Collections;

/// <summary>
/// Asteroid script. This script is responsible for making an object orbit around a point (Globe)
/// it will require the rigidbody component, to apply constant force toward the point,
/// and it will start with a random velocity not pointing at the point (at least orthogonal).
/// </summary>
public class AsteroidScript : MonoBehaviour {
	Transform pointOfOrbit;
	public void setPointOfOrbit(Transform point){ pointOfOrbit = point; }

	public float startVelocityMin, startVelocityMax;
	public float startHeightMin, startHeightMax;
	public float gravityFactorMin, gravityFactorMax;
	public float maxSpeed;

	/// <summary>
	/// each time the orbit object hits the bounce, it will rebound upward with normal and bounceAddBack
	/// </summary>
	public float bounceAddBack;

	float startVelocity, startHeight, gravity;

	void Start(){
		//pick random parameters;
		startVelocity = Random.Range (startVelocityMin, startVelocityMax);
		startHeight = Random.Range (startHeightMin, startHeightMax);
		gravity = Random.Range (gravityFactorMin, gravityFactorMax);

		//pick random place around the point;
		Vector3 randomVector = new Vector3(
			Random.Range (-10, 10),
			Random.Range (-10, 10),
			Random.Range (-10, 10)
			).normalized;

		//make sure c is not 0
		if( randomVector.z == 0 ){
			randomVector.z += 1;
		}

		//apply height;
		randomVector = randomVector * startHeight;

		//add to the orbitPoint, and we got the starting point;
		Vector3 startPoint = pointOfOrbit.position + randomVector;
		transform.position = startPoint;

		//now we need a random direction for our velocity.
		float x, y, z;
		x = Random.Range (-10,10);
		y = Random.Range (-10, 10);

		//z = a*x +b*y / -c;   c must not be zero.
		z = -(randomVector.x * x + randomVector.y * y) / randomVector.z;

		Vector3 randomDirection = new Vector3(x, y, z).normalized * startVelocity;

		GetComponent<Rigidbody>().velocity = randomDirection;
		//rigidbody.velocity = Vector3.forward;
	
	}

	public void myUpdate(){
		if( !isStopped ){
			GetComponent<Rigidbody>().AddForce ( (pointOfOrbit.position - transform.position).normalized * gravity *Time.deltaTime);
		}

		if( GetComponent<Rigidbody>().velocity.magnitude > maxSpeed ){
			GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;
		}
	}

	bool isStopped = false;
	public void stop(){
		gravity = 0;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public void bounce(Transform other){
		//Debug.Log (other.position);
		Vector3 normal = (transform.position - other.position).normalized;
		Vector3 velocity = GetComponent<Rigidbody>().velocity;
		GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + ( normal * ((
			-velocity.x * normal.x 
			-velocity.y * normal.y 
			-velocity.z * normal.z ) + bounceAddBack));
	}
}
