using System;
using UnityEngine;

public class BallRenderer
{
    private Person _person;
    private SpriteRenderer _healthRenderer;
    private SpriteRenderer _immunityRenderer;
    internal BallRenderer(Ball b, Person p)
    {
        _healthRenderer = b.gameObject.GetComponent<SpriteRenderer>();
        _immunityRenderer = b.transform.Find("Immunity Level").GetComponent<SpriteRenderer>();
        _person = p;
    }

    internal void UpdateColorsBasedOnPerson()
    {
        if (!_person.IsDead)
        {
            if (_person.ImmuneLevel > 0.25)
            {
                _immunityRenderer.color = Color.black;
            }
            else
            {
                _immunityRenderer.color = Color.clear;
            }
            if (_person.HealthLevel > 0.25)
            {
                _healthRenderer.color = Color.green;
            }
            else
            {
                _healthRenderer.color = new Color(32, 128, 32, 1); ;
            }
        }
        if (_person.IsInfected)
        {
            _healthRenderer.color = Color.red;
        }
        if (_person.IsRecovering)
        {
            _healthRenderer.color = Color.yellow;
        }
        if (_person.IsDead)
        {
            _healthRenderer.color = Color.gray;
        }
    }
}
