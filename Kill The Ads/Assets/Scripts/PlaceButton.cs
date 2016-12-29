using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlaceButton : MonoBehaviour
{
    void Start()
    {
        var collider2D = gameObject.GetComponent<BoxCollider2D>();
        var renderer = gameObject.GetComponent<Renderer>();
        float width = (renderer.bounds.size.x / 2) - (collider2D.size.x / 2);
        float height = (renderer.bounds.size.y / 2) - (collider2D.size.y / 2);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(width, height);
    }

    void OnMouseDown()
    {
        print("I was clicked");
        Destroy(gameObject);
    }
}