using System;
internal class Person
{
    private PersonState _state;

    private double _healthLevel;
    private double _immuneLevel;
    private double _recoveryRate;
    private double _immunityLossRate;
    private int _incubationPeriod;
    private int _lastInfection;

    private readonly Random _random = new Random();
    private readonly SimulationWorld _parentWorld;
        public Person(SimulationWorld parentWorld)
    {
        if (parentWorld == null)
            throw new ArgumentNullException("parentWorld");
        _parentWorld = parentWorld;
        _healthLevel = _random.Next(75, 100) / 100.0; // initial health level 0.75 to 1.0
        _immuneLevel = _random.Next(1, 10) / 100.0; // initial immune level 0.01 to 0.1
        _recoveryRate = _random.Next(5, 10) / 100.0; // recovery rate 0.05 to 0.1
        _immunityLossRate = _random.Next(2, 5 )/ 100.0; // immunity loss rate 0.02 to 0.05
        _incubationPeriod = _random.Next(15000, 60000); // incubation period in miliseconds
        _state = PersonState.Healthy;
    }
    public bool IsHealthy { get => _state == PersonState.Healthy; }
    public bool IsInfected { get => _state == PersonState.Infected; }
    public bool IsRecovering { get => _state == PersonState.Recovering; }
    public bool IsDead { get => _state == PersonState.Dead; }
    public double HealthLevel { get => _healthLevel; }
    public double ImmuneLevel { get => _immuneLevel; }

    internal void InitializeHealth()
    {
        double chance = _random.Next(0, 100) / 100.0;
        if (chance < _parentWorld.InitialInfectionRate)
        {
            this.Infect();
        }
    }

    internal void Infect()
    {
        _lastInfection = Environment.TickCount;
        _state = PersonState.Infected;
        _healthLevel = applyDecayStep(_healthLevel, this._parentWorld.LethalityRate);
    }

    public double RecoveryRate
    {
        get => _recoveryRate;
        set
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("recoveryRate");
                _recoveryRate = value;
        }
    }

    public double ImmunityLossRate
    {
        get => _immunityLossRate;
        set
        {
            if (value < 0.0 || value > 1.0)
                throw new ArgumentOutOfRangeException("immunityLossRate");
                _immunityLossRate = value;
        }
    }

    public void ApplyContact()
    {
        if (IsDead) return;
        double chance = _random.Next(0, 100) / 100.0;
        if (chance < this._parentWorld.Transmissibility)
        {
            this.Infect();
        }
    }

    public void UpdateHealth()
    {
        if (IsDead) return;
        int currentTime = Environment.TickCount;
        if (this.IsInfected)
        {
            if ((currentTime - _lastInfection) > _incubationPeriod * (1- _immuneLevel))
                {
                _lastInfection = 0;
                _state = PersonState.Recovering;
            }
            else
            {
                _healthLevel = applyDecayStep(_healthLevel, this._parentWorld.LethalityRate);
                _immuneLevel = applyGrowthStep(_immuneLevel, _recoveryRate);
            }
            if (_healthLevel < 0.2)
            {
                _state = PersonState.Dead;
            }
        }
        if (_state == PersonState.Recovering)
        {
            _healthLevel = applyGrowthStep(_healthLevel, _recoveryRate);
            _immuneLevel = applyGrowthStep(_immuneLevel, _recoveryRate);
            if (_healthLevel >= 0.9)
            {
                _state = PersonState.Healthy;
            }
        }
        if (_state == PersonState.Healthy)
        {
            _immuneLevel = applyDecayStep(_immuneLevel, _immunityLossRate);
        }

    }

    private double applyGrowthStep(double originalValue, double growthRate)
    {
        return originalValue + (1 - originalValue) * growthRate;
    }

    private double applyDecayStep(double originalValue, double decayRate)
    {
        return originalValue - originalValue * decayRate;
    }
}

internal enum PersonState
{
    Healthy,
    Infected,
    Recovering,
    Dead
}