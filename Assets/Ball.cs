using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopupUI;

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
    private bool _isPopupOn = false;
    private FollowObject _popupUI;
    [SerializeField]
    double healthLevel;
    [SerializeField]
    double immunityLevel;
    // Awake is called on object inititalization
    void Awake()
    {
        _camera = Camera.main;

        _moveDirection = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value);
        _popupUI = GameObject.Find("Popup Display").transform.Find("Popup Canvas").GetComponent<FollowObject>();
        _moveDirection.Normalize();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _person = new Person(_world);
        _renderer = new BallRenderer(this, _person);
        _rb.velocity = _moveDirection * _world.Mobility;
    }

    internal void SetHealth(PersonState state)
    {
        _person.InitializeHealth();
        if( state == PersonState.Infected)
        {
            _person.Infect();
        }
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

    private void OnMouseDown()
    {
        //Ball is clicked
        if (_isPopupOn)
        {
            _isPopupOn = false;
            _popupUI.SetPopupActive(false);
        }
        else
        {
            _isPopupOn = true;
            _popupUI.SetTarget(this.gameObject);
            _popupUI.SetPopupActive(true);
        }
       
    }

    public double GetHealth()
    {
        return _person.HealthLevel;
    }

    public double GetImmunityLevel()
    {
        return _person.ImmuneLevel;
    }


    public void OnTransimissibilityChange(Single value)
    {
        this._world.OnTransimissibilityChange(value);
    }

    public void OnLethalityChange(Single value)
    {
        this._world.OnLethalityChange(value);
    }

    public void OnMobilityChange(Single value)
    {
        this._world.OnMobilityChange(value);
    }
}
