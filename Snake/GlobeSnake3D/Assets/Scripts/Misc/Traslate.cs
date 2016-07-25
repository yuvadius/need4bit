using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Traslate : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = Translator.GetTranslation(text.text);
        Destroy(this);
	}

}
