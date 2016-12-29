using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PlaceButton : MonoBehaviour
{
    public static int n_ads = 0;

    void Start()
    {
        var collider2D = gameObject.GetComponent<BoxCollider2D>();
        var renderer = gameObject.GetComponent<Renderer>();
        float width = (renderer.bounds.size.x / 2) - (collider2D.size.x / 2);
        float height = (renderer.bounds.size.y / 2) - (collider2D.size.y / 2);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(width, height);
        ++n_ads;

        Debug.Log(n_ads);

        //if there are enough ads, lose
        if (n_ads >= 5)
        {
            SceneManager.LoadScene("GameOver");
            n_ads = 0;
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
        --n_ads;
    }
}