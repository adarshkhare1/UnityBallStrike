using System;
internal class Person
{
    private const int IncubationPeriodMin = 5000;
    private const int IncubationPeriodMax = 65000;

    private const float HealthLevelMin = 0.7f;
    private const float HealthLevelMax = 1.0f;

    private const float ImmuneLevelMin = 0.1f;
    private const float ImmuneLevelMax = 0.3f;

    private const float ImmunityLossRateMin = 0.005f;
    private const float ImmunityLossRatelMax = 0.02f;

    private const float RecoveryRateMin = 0.05f;
    private const float RecoveryRatelMax = 0.20f;

    private PersonState _state;

    private double _healthLevel;
    private double _immuneLevel;
    private int _incubationPeriod;
    private int _lastInfection;

    private readonly Random _random = new Random();
    private readonly SimulationWorld _parentWorld;
        public Person(SimulationWorld parentWorld)
    {
        if (parentWorld == null)
            throw new ArgumentNullException("parentWorld");
        _parentWorld = parentWorld;
        _healthLevel = NormalDistribution.GetRandomGaussian(HealthLevelMin, HealthLevelMax);
        _immuneLevel = NormalDistribution.GetRandomGaussian(ImmuneLevelMin, ImmuneLevelMax);
        _incubationPeriod = IncubationPeriodMin; // incubation period in miliseconds
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
        _incubationPeriod = (int) NormalDistribution.GetRandomGaussian(IncubationPeriodMin, IncubationPeriodMax);
        _lastInfection = Environment.TickCount;
        _state = PersonState.Infected;
        _healthLevel = applyDecayStep(_healthLevel, this._parentWorld.LethalityRate);
    }

    public void ApplyContact()
    {
        if (IsDead) return;
        double safetyChance = _random.Next(0, 100) / 100.0;
        if (safetyChance < this._parentWorld.Transmissibility && ImmuneLevel < this._parentWorld.Transmissibility)
        {
            this.Infect();
        }
    }

    public void UpdateHealth()
    {
        if (IsDead) return;
        int currentTime = Environment.TickCount;
        float recoveryRate = NormalDistribution.GetRandomGaussian(RecoveryRateMin, RecoveryRatelMax);
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
                _immuneLevel = applyGrowthStep(_immuneLevel, recoveryRate);
            }
            if (_healthLevel < 0.2)
            {
                _state = PersonState.Dead;
            }
        }
        if (_state == PersonState.Recovering)
        {
            _healthLevel = applyGrowthStep(_healthLevel, recoveryRate);
            _immuneLevel = applyGrowthStep(_immuneLevel, recoveryRate);
            if (_healthLevel >= 0.9)
            {
                _state = PersonState.Healthy;
            }
        }
        if (_state == PersonState.Healthy)
        {
            // Set future immunity Loss rate
            float immunityLossRate = NormalDistribution.GetRandomGaussian(ImmunityLossRateMin, ImmunityLossRatelMax);
            _immuneLevel = applyDecayStep(_immuneLevel, immunityLossRate);
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