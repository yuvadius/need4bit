using UnityEngine;
using System.Collections;

public class CreditsRoll : MonoBehaviour {

    public RectTransform[] credits;
    public RectTransform start;
    public float speed;
    public float time;

    void Start() {
        float deltaY = credits[0].position.y - start.position.y;

        for (int i=0; i<credits.Length; ++i) {
            credits[i].transform.position = new Vector3(
                credits[i].transform.position.x,
                credits[i].transform.position.y - deltaY,
                credits[i].transform.position.z
            );
        }
    }

    public void StartRolling() {
        StopAllCoroutines();
        StartCoroutine(creditRoll());
    }

    IEnumerator creditRoll() {
        Start();

        float timePassed = 0;

        while (timePassed < time) {

            float move = Time.deltaTime * speed;

            for (int i=0; i<credits.Length; ++i) {
                credits[i].transform.position = new Vector3(
                    credits[i].transform.position.x,
                    credits[i].transform.position.y + move,
                    credits[i].transform.position.z
                );
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
    }

}
