using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal") * speed;
        float moveV = Input.GetAxis("Vertical") * speed;
        rb.AddForce(new Vector2(moveH, moveV));
        if (Input.GetKeyDown("space") && (lastTime == 0 || Time.fixedTime - lastTime >= timeWait))
        {
            Instantiate(Resources.Load("Dynamite"), gameObject.transform.position, gameObject.transform.rotation);
            lastTime = Time.fixedTime;
        }
    }
}
