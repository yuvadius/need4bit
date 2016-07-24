using UnityEngine;
using System.Collections;

public class DynamiteExplode : Photon.MonoBehaviour
{
    public float timeDestroy;
	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyBarrel(timeDestroy));
	}

    IEnumerator DestroyBarrel(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
}
