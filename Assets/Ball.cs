using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float _speed = 1f;
    private Camera _camera;
    private Rigidbody2D _rb;
    private int _tickCount;
    Vector3 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _moveDirection = new Vector3(Random.value, Random.value);
        _moveDirection.Normalize();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = _moveDirection * _speed;
        _tickCount = System.Environment.TickCount;

    }

    private void Update()
    {
        if((System.Environment.TickCount-_tickCount) > 1000)
        {
            _speed++;
            _tickCount = System.Environment.TickCount;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _moveDirection = Vector3.Reflect(_moveDirection, collision.contacts[0].normal);
        _rb.velocity = _moveDirection * _speed;
    }
}
