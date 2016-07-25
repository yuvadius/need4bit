using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Languages{
    public string russian, english;
}

[System.Serializable]
public class TwoStrings {
    public string key;
    public Languages value;
}

public class Translator : MonoBehaviour {

    public TwoStrings[] texts;

    static Dictionary<string, string> lookUpTable = new Dictionary<string, string>();

    void Awake() {

        lookUpTable = new Dictionary<string, string>();

        if (Application.systemLanguage == SystemLanguage.Russian ||
            Application.systemLanguage == SystemLanguage.Ukrainian ||
            Application.systemLanguage == SystemLanguage.Belarusian ||
            Application.systemLanguage == SystemLanguage.Unknown ||
            Application.systemLanguage == SystemLanguage.Turkish ||
            Application.systemLanguage == SystemLanguage.Arabic ||
            Application.systemLanguage == SystemLanguage.Polish ||
            Application.systemLanguage == SystemLanguage.Latvian ||
            Application.systemLanguage == SystemLanguage.Hebrew ||
            Application.systemLanguage == SystemLanguage.Estonian ||
            Application.systemLanguage == SystemLanguage.Czech ||
            Application.systemLanguage == SystemLanguage.Bulgarian) {

            for (int i=0; i<texts.Length; ++i) {
                lookUpTable.Add(texts[i].key, texts[i].value.russian);
            }

        } else {
            for (int i=0; i<texts.Length; ++i) {
                lookUpTable.Add(texts[i].key, texts[i].value.english);
            }
        }

        texts = null;
    }

    public static string GetTranslation(string key) {
        if (lookUpTable.ContainsKey(key)) {
            return lookUpTable[key];
        }
        return "no translation";
    }
}
