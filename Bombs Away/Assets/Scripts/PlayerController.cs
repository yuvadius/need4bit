using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody2D rb;
    private float lastTime = 0;
    //The time to wait untill another dynamite can be created
    public float timeWait;
    public float speed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.green;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        float moveH = Input.GetAxis("Horizontal") * speed;
        float moveV = Input.GetAxis("Vertical") * speed;
        rb.AddForce(new Vector2(moveH, moveV));
        if (Input.GetKeyDown("space") && (lastTime == 0 || Time.fixedTime - lastTime >= timeWait))
        {
            CmdCreateDynamite();
            lastTime = Time.fixedTime;
        }
    }

    [Command]
    void CmdCreateDynamite()
    {
        GameObject dynamite = Instantiate(Resources.Load("Dynamite"), gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        // Spawn the Dynamite on the Clients
        NetworkServer.Spawn(dynamite);
    }
}
