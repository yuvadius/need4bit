using UnityEngine;
using System.Collections;

public class SnakeFixedInput : Photon.MonoBehaviour
{
    void Start()
    {
        /*if (photonView.isMine)
            gameObject.SetActive(false);*/
    }

	void Update () {
        if (photonView.isMine)
        {
            Transform snake = SnakeInstance.instance.transform;
            transform.position = snake.position;
            transform.rotation = snake.rotation;
            transform.localScale = snake.localScale;
            transform.GetChild(0).position = snake.GetChild(0).position;
            transform.GetChild(0).rotation = snake.GetChild(0).rotation;
            transform.GetChild(0).localScale = snake.GetChild(0).localScale;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.localScale);

            stream.SendNext(transform.GetChild(0).position);
            stream.SendNext(transform.GetChild(0).rotation);
            stream.SendNext(transform.GetChild(0).localScale);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();

            transform.GetChild(0).position = (Vector3)stream.ReceiveNext();
            transform.GetChild(0).rotation = (Quaternion)stream.ReceiveNext();
            transform.GetChild(0).localScale = (Vector3)stream.ReceiveNext();
        }
    }
}
