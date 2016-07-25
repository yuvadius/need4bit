using UnityEngine;
using System.Collections;

/// <summary>
/// This script will play the role of communication with the controller and the environment.
/// </summary>
public class Bannana : MonoBehaviour {
	BannanaController controller;
	AsteroidScript spinner;

	public void setController(BannanaController controller){
		this.controller = controller;
	}

	//this is only called once at initialization.
	public void setPointOfOrbit(Transform point){
		spinner = GetComponent<AsteroidScript>();
		spinner.setPointOfOrbit(point);
	}

	public void myUpdate(){
		spinner.myUpdate();
	}

	public void eat(Transform head){
		spinner.stop();
		controller.destroy (this);
		StartCoroutine(getEaten(head, 1.0f));
	}

	IEnumerator getEaten(Transform head, float seconds){
		float time = seconds;
		float distance = (transform.position - head.position).magnitude;

		while( time > 0 ){


			float currentDistance = time * distance / seconds;

			transform.position = head.position - ((head.position - transform.position).normalized * currentDistance);

			time -= Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}


	public void bounce(Transform other){
		spinner.bounce(other);
	}
}
