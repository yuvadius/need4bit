using UnityEngine;
using System.Collections;

public class BiteScript : MonoBehaviour {

    Animator anim;
    AudioSource aud;

    public float soundOffset;

    void Start() {
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
    }

	public void bite(){
        anim.SetTrigger("Bite");
        StartCoroutine(bitePlayCo());
	}

    IEnumerator bitePlayCo() {
        yield return new WaitForSeconds(soundOffset);
        aud.Play();
    }
}
