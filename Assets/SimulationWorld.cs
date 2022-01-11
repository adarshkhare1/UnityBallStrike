using System;
using UnityEngine;
public class SimulationWorld
{
    public static SimulationWorld World = new SimulationWorld();
    public readonly float InitialInfectionRate = 0.01f;
    private float _transmissibility;
    private float _lethalityRate;
    private float _mobility;

    private SimulationWorld()
    {
        _transmissibility = 0.75f;
        _lethalityRate = 0.05f;
        _mobility = 1.0f;
    }

    public float Transmissibility
    {
        get => _transmissibility;
        set
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("transmissibility");
            _transmissibility = value;
        }
    }

    public float LethalityRate
    {
        get => _lethalityRate;
        set
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("fatality");
            _lethalityRate = value;
        }
    }

    public float Mobility
    {
        get => _mobility;
        set
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("movability");
            _mobility = value;
        }
    }

    internal void OnTransimissibilityChange(float value)
    {
        _transmissibility = (float)value/100.0f;
        Debug.Log("Transmissibility=" + _transmissibility);
    }

    internal void OnLethalityChange(float value)
    {
        _lethalityRate = (float)value / 500.0f;
        Debug.Log("Lethality=" + _lethalityRate);
    }

    internal void OnMobilityChange(float value)
    {
        _mobility = (float)value / 20.0f;
        Debug.Log("Mobility=" + _mobility);
    }
}
