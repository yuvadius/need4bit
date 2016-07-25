using UnityEngine;
using System.Collections;

public class AppleAppear : MonoBehaviour {

	public AppleCollide appleCollider;

	void OnEnable() {
		appleCollider.enabled = false;
        StartCoroutine(appear(1.0f));
    }

    IEnumerator appear(float seconds) {
        float time = 0.0f;

		float halfTime = seconds / 2;

        while (time < halfTime) {
            time += Time.deltaTime;
            float value = time / seconds;
            transform.localScale = new Vector3(value, value, value);
            yield return null;
        }

		appleCollider.enabled = true;

		while (time < seconds)
		{
			time += Time.deltaTime;
			float value = time / seconds;
			transform.localScale = new Vector3(value, value, value);
			yield return null;
		}

		transform.localScale = Vector3.one;
    }

}
