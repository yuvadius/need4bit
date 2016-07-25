using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WelcomeTextFader : MonoBehaviour {

    public float fadeOutTime, fadeInTime, outTime, inTime;
    public Color color;
    Text label;
    bool shouldFade = true;

    IEnumerator Start() {
        label = GetComponent<Text>();

        label.color = new Color(1, 1, 1, 0);

        float timePassed;

        while (shouldFade) {

            yield return new WaitForSeconds(outTime);

            timePassed = 0;
            while (timePassed < fadeInTime) {
                label.color = new Color(color.r, color.g, color.b, timePassed / fadeInTime);
                timePassed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(inTime);

            timePassed = 0;
            while (timePassed < fadeOutTime) {
                label.color = new Color(color.r, color.g, color.b, 1 - timePassed / fadeInTime);
                timePassed += Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
        Destroy(gameObject);
    }

    public void StopFading() {
        shouldFade = false;
    }

}
