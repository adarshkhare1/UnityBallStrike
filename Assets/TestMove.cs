using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{

    public float speed = 15f;

    // Update is called once per frame
    void Update()
    {
        float factor = Time.deltaTime * speed;
        Vector3 delta = Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow)) {
            delta += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            delta += Vector3.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            delta += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            delta += Vector3.right;
        }

        delta.Normalize();
        transform.position += delta * factor;
    }
}
