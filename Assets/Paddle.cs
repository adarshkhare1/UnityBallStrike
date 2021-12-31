using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    [SerializeField]
    private float _speed = 10f;
    private Camera _camera;


    // Start is called before the first frame update
    void Start() {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
        this.AdjustForBoundaryEdges();
    }

    private void Move()
    {
        Vector3 transform = new Vector3(0, Time.deltaTime * _speed);

        if (Input.GetKey(KeyCode.UpArrow)) {
            this.transform.position += transform;
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            this.transform.position -= transform;
        }
    }

    private void AdjustForBoundaryEdges()
    {
        float currentX = this.transform.position.x;
        float currentY = this.transform.position.y;
        float clampLimit = _camera.orthographicSize - this.transform.localScale.y / 2;
        this.transform.position
            = new Vector3(currentX, Mathf.Clamp(currentY, -clampLimit, clampLimit));
    }
}
