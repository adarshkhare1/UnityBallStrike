using System;
internal class SimulationWorld
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
}
