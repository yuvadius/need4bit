using UnityEngine;
using System.Collections;

public enum SnakeColor
{
    GREEN,
    BLUE,
    RED,
    YELLOW
}

public class SnakeColorControl : MonoBehaviour {

    public Material greenHead, greenSeg, blueHead, blueSeg, redHead, redSeg, yellowHead, yellowSeg;

    public MeshRenderer headRenderer, segmentRenderer;

    public void SetColor(SnakeColor color)
    {
        switch (color)
        {
            case SnakeColor.BLUE:

                break;
            case SnakeColor.GREEN:

                break;
            case SnakeColor.RED:

                break;
            case SnakeColor.YELLOW:

                break;
        }
    }
}
