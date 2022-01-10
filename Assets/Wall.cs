using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private enum WallLocation
    {
        Top,
        Bottom,
        Left,
        Right
    }
    [SerializeField]
    WallLocation location;
    // Start is called before the first frame update
    void Start()
    {        
       
    }

    // Update is called once per frame
    void Update()
    {
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        float wallWidth = 0.5f;
        switch (location)
        {
            case WallLocation.Top:
                collider.size = new Vector2(horzExtent*2, 2*wallWidth);
                collider.transform.position = new Vector3(0, vertExtent + wallWidth - 1.5f);
                break;
            case WallLocation.Bottom:
                collider.size = new Vector2(horzExtent*2, 1);
                collider.transform.position = new Vector3(0, -vertExtent - wallWidth);
                break;
            case WallLocation.Left:
                collider.size = new Vector2(1, vertExtent*2);
                collider.transform.position = new Vector3(-horzExtent - wallWidth, 0);
                break;
            case WallLocation.Right:
                collider.size = new Vector2(1, vertExtent*2);
                collider.transform.position = new Vector3(horzExtent + wallWidth, 0);
                break;
        }
    }
}
