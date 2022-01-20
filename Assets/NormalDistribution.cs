using System;
using UnityEngine;

public class NormalDistribution
{
    private float _variance = 0.05f;
    private float _mean;


    public static float GetRandomGaussian(float minValue, float maxValue)
    {
        NormalDistribution nd = new NormalDistribution(minValue, maxValue);
        return nd.RandomGaussian();
    }

    public NormalDistribution(float minValue = 0.0f, float maxValue = 1.0f)
    {
        _mean = (minValue+ maxValue) / 2;
        _variance = (maxValue - Mean ) / 3.0f;
    }

    public NormalDistribution(float mean)
    {
        _mean = mean;
    }

    internal float Mean { get => _mean; }

    internal float RandomGaussian()
    {
        float x1, x2, w, y1; //, y2;

        do
        {
            x1 = 2f * (float)UnityEngine.Random.value - 1f;
            x2 = 2f * (float)UnityEngine.Random.value - 1f;
            w = x1 * x1 + x2 * x2;
        } while (w >= 1f);

        w = Mathf.Sqrt((-2f * Mathf.Log(w)) / w);
        y1 = x1 * w;
        return (y1 * _variance) + _mean;
    }
}

