using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
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
    }
}
