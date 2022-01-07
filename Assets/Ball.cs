using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private SimulationWorld _world = SimulationWorld.World;
    private Person _person;
    private float _speed = 1f;
    private const float MaxSpeed = 10f;
    private Camera _camera;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private int _tickCount;
    private Vector3 _moveDirection;
    private float _timer = 0;
    
    // Awake is called on object inititalization
    void Awake()
    {
        _camera = Camera.main;
        _moveDirection = new Vector3(Random.value, Random.value);
        _moveDirection.Normalize();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
        _person = new Person(_world);
        _rb.velocity = _moveDirection * _world.Mobility;
        _tickCount = System.Environment.TickCount;

    }

    public void SetHealth()
    {
        _person.InitializeHealth();
        this.SetRenderColorBasedOnPersonState();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= 1.0)
        {
            _person.UpdateHealth();
            this.SetRenderColorBasedOnPersonState();
            if(_person.IsDead)
            {
                _rb.velocity = _rb.velocity * 0.0f;
            }
            _timer = 0;
        }

        //if(_speed < MaxSpeed > 1000)
        //{
        //    _speed = _speed+0.1f;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BallTag"))
        {
            if (_person.IsHealthy)
            {
                Ball otherBall = collision.gameObject.GetComponent<Ball>();
                if (otherBall._person.IsInfected)
                {
                    _person.ApplyContact();
                    this.SetRenderColorBasedOnPersonState();
                }
            }
        }
        _moveDirection = Vector3.Reflect(_moveDirection, collision.contacts[0].normal);
        _rb.velocity = _moveDirection * _speed;
        
    }

    private void SetRenderColorBasedOnPersonState()
    {
        if (_person.IsInfected)
        {
            _renderer.color = Color.red;
        }
        if (_person.IsRecovering)
        {
            _renderer.color = Color.yellow;
        }
        if (_person.IsHealthy)
        {
            _renderer.color = Color.green;
        }
        if (_person.IsDead)
        {
            _renderer.color = Color.gray;
        }
    }
}
