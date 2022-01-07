using System;
internal class Person
{
    private PersonState _state;

    private double _healthLevel;
    private double _immuneLevel;
    private double _recoveryRate;
    private double _immunityLossRate;
    private int _recoveryPeriod;
    private int _lastInfection;

    private readonly Random _random = new Random();
    private readonly SimulationWorld _parentWorld;
        public Person(SimulationWorld parentWorld)
    {
        if (parentWorld == null)
            throw new ArgumentNullException("parentWorld");
        _parentWorld = parentWorld;
        _healthLevel = 1.0f;
        _immuneLevel = 0.0f;
        _recoveryRate = 0.05;
        _immunityLossRate = 0.1f;
        _recoveryPeriod = 30000;
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

    private void Infect()
    {
        _lastInfection = Environment.TickCount;
        _state = PersonState.Infected;
        _healthLevel = applyLogarithmDecayStep(_healthLevel, this._parentWorld.LethalityRate);
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
        if (_immuneLevel < chance/2 &&
            chance < this._parentWorld.Transmissibility)
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
            if ((currentTime - _lastInfection) > _recoveryPeriod)
                {
                _lastInfection = 0;
                _state = PersonState.Recovering;
            }
            else
            {
                _healthLevel = applyLogarithmDecayStep(_healthLevel, this._parentWorld.LethalityRate);
                _immuneLevel = applyLogisticGrowthStep(_immuneLevel, _immunityLossRate);
            }
            if (_healthLevel < 0.2)
            {
                _state = PersonState.Dead;
            }
        }
        if (_state == PersonState.Recovering)
        {
            _healthLevel = applyLogisticGrowthStep(_healthLevel, _recoveryRate);
            _immuneLevel = applyLogisticGrowthStep(_immuneLevel, _recoveryRate);
            if (_healthLevel >= 0.9)
            {
                _state = PersonState.Healthy;
            }
        }
        if (_state == PersonState.Healthy)
        {
            _immuneLevel = applyLogarithmDecayStep(_immuneLevel, _immunityLossRate);
        }

    }

    private double applyLogisticGrowthStep(double originalValue, double growthRate)
    {
        return 1 / (1 + (1 - originalValue) * Math.Exp(growthRate));
    }

    private double applyLogarithmDecayStep(double originalValue, double decayRate)
    {
        return (1 - 1 / (1 + (1 - originalValue) * Math.Exp(decayRate)));
    }
}

internal enum PersonState
{
    Healthy,
    Infected,
    Recovering,
    Dead
}