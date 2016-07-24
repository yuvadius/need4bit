using UnityEngine;
using Photon;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour
{
    private Rigidbody2D rb;
    private float lastTime = 0;
    //The time to wait untill another dynamite can be created
    public float timeWait;
    public float speed;

    //Variables needed for interpolation
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector2 syncStartPosition = Vector2.zero;
    private Vector2 syncEndPosition = Vector2.zero;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (photonView.isMine)
            InputColorChange();
    }

    void Update()
    {
        if (photonView.isMine && Input.GetKeyDown("space") && (lastTime == 0 || Time.fixedTime - lastTime >= timeWait))
        {
            CreateDynamite();
            lastTime = Time.fixedTime;
        }
    }

    void FixedUpdate()
    {
        if (photonView.isMine)
        {
            float moveH = Input.GetAxis("Horizontal") * speed;
            float moveV = Input.GetAxis("Vertical") * speed;
            rb.AddForce(new Vector2(moveH, moveV));
        }
        else
        {
            SyncedMovement();
        }
    }

    private void InputColorChange()
    {
        ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }

    [PunRPC]
    void ChangeColorTo(Vector3 color)
    {
        GetComponent<SpriteRenderer>().material.color = new Color(color.x, color.y, color.z, 1f);

        if (photonView.isMine)
            photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, color);
    }

    void CreateDynamite()
    {
        PhotonNetwork.Instantiate("Dynamite", gameObject.transform.position, gameObject.transform.rotation, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector2 syncPosition = (Vector2)stream.ReceiveNext();
            Vector2 syncVelocity = (Vector2)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = transform.position;
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        transform.position = Vector2.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }
}
