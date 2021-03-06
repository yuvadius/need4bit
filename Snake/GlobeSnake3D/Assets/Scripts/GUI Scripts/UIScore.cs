using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScore : MonoBehaviour {

    public Text appleCountLbl;
    public Text bannanaCountLbl;

    int appleCount = 0;
    int bannanaCount = 0;

    void Start() {
        appleCountLbl.text = "";
        bannanaCountLbl.text = "";
    }

    public void AddApple() {
        appleCountLbl.text = (++appleCount).ToString();
        if (PhotonNetwork.offlineMode == false)
            PhotonNetwork.player.AddScore(1);
    }

    public void AddBannana() {
        bannanaCountLbl.text = (++bannanaCount).ToString();
    }
}
