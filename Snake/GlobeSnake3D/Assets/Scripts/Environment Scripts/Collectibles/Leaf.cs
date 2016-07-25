using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

    public Transform father, apple;

    public float timeToWait;

    public float jumpMinStrength, jumpMaxStrength;
    public float gravityMinStrength, gravityMaxStrength;
    public float outDistance;

    Vector3 velocity;
    float gravity;

	Vector3 startingPos;
	Quaternion startingRot;

	void Start()
	{
		startingPos = transform.localPosition;
		startingRot = transform.localRotation;
	}

    public void DropOff() {
        StartCoroutine(dropOffCo());
    }

	public void AttachBack()
	{
		transform.SetParent(apple);
		transform.localPosition = startingPos;
		transform.localRotation = startingRot;
		transform.localScale = Vector3.one;
		gameObject.SetActive(true);
	}

    IEnumerator dropOffCo() {


        velocity = (((father.position - transform.position) + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))) * -1).normalized * Random.Range(jumpMinStrength, jumpMaxStrength);
        gravity = Random.Range(gravityMinStrength, gravityMaxStrength);

        Vector3 direction;

        yield return new WaitForSeconds(timeToWait);

        transform.SetParent(father);

        while ((transform.position - father.position).sqrMagnitude > outDistance) {

            direction = father.position - transform.position;
            direction = direction.normalized;

            velocity = velocity + direction * gravity * Time.deltaTime;

            transform.position = transform.position + velocity * Time.deltaTime;

            yield return null;
        }

		gameObject.SetActive(false);
    }

}
