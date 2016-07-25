using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipTextFader : MonoBehaviour {

    public float tipIn, tipStay, tipOut, tipRest;

    public string[] tips;

    public Color color;
    Text label;

    void Start() {
        label = GetComponent<Text>();
        label.color = new Color(1, 1, 1, 0);
    }

    public void StartTipping() {

        for (int i=0; i<tips.Length; ++i) {
            tips[i] = Translator.GetTranslation(tips[i]);
        }

        StopAllCoroutines();
        StartCoroutine(tipCo());
    }

    IEnumerator tipCo() {

        

        float timePassed;

        int index = 0;
        while (index < tips.Length) {

            label.text = tips[index];

            yield return new WaitForSeconds(tipRest);

            timePassed = 0;
            while (timePassed < tipIn) {
                label.color = new Color(color.r, color.g, color.b, timePassed / tipIn);
                timePassed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(tipStay);

            timePassed = 0;
            while (timePassed < tipOut) {
                label.color = new Color(color.r, color.g, color.b, 1 - timePassed / tipOut);
                timePassed += Time.deltaTime;
                yield return null;
            }

            label.color = new Color(1, 1, 1, 0);
            index++;
        }


        Destroy(gameObject);
    }
}
