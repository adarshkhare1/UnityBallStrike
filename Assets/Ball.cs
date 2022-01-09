using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private SimulationWorld _world = SimulationWorld.World;
    private Person _person;
    private BallRenderer _renderer;
    private float _speed = 1f;
    private Camera _camera;
    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    private float _timer = 0;
    [SerializeField]
    double healthLevel;
    [SerializeField]
    double immunityLevel;
    // Awake is called on object inititalization
    void Awake()
    {
        _camera = Camera.main;
        _moveDirection = new Vector3(Random.value, Random.value);
        _moveDirection.Normalize();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _person = new Person(_world);
        _renderer = new BallRenderer(this, _person);
        _rb.velocity = _moveDirection * _world.Mobility;
    }

    public void SetHealth()
    {
        _person.InitializeHealth();
        _renderer.UpdateColorsBasedOnPerson();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        healthLevel = _person.HealthLevel;
        immunityLevel = _person.ImmuneLevel;
        if (_timer >= 1.0)
        {
            _person.UpdateHealth();
            _renderer.UpdateColorsBasedOnPerson();
            if (_person.IsDead)
            {
                _rb.velocity *= 0.0f;
            }
            else if (_person.IsInfected)
            {
                _rb.velocity = _moveDirection * _world.Mobility/2;
            }
            else
            {
                _rb.velocity = _moveDirection * _world.Mobility;
            }
            _timer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("BallTag"))
        {
            var _childSpr = this.transform.Find("Immunity Level").GetComponent<SpriteRenderer>();
            if (_person.IsHealthy)
            {
                Ball otherBall = collision.gameObject.GetComponent<Ball>();
                if (otherBall._person.IsInfected)
                {
                    _person.ApplyContact();
                    _renderer.UpdateColorsBasedOnPerson();
                }
            }
        }
        _moveDirection = Vector3.Reflect(_moveDirection, collision.contacts[0].normal);
        _rb.velocity = _moveDirection * _speed;
        
    }
}
