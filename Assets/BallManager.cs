using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private int _populationSize = 200;
    private Camera _camera;
    private float _clampLimit;
    [SerializeField]
    GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _clampLimit = _camera.orthographicSize - this.transform.localScale.y / 2;
        for (int i = 1; i <= _populationSize; i++)
        {
            GameObject b = LaunchBall();
            b.GetComponent<Ball>().SetHealth(PersonState.Healthy);
        }
    }

    internal GameObject LaunchBall()
    {
        float x = Random.Range(-_clampLimit-7.5f, +_clampLimit);
        float y = Random.Range(-_clampLimit, +_clampLimit);
        GameObject b = Instantiate(ballPrefab, new Vector3(x, y), Quaternion.identity);
        return b;
    }

    public void AddInfected()
    {
        GameObject b = this.LaunchBall();
        b.GetComponent<Ball>().SetHealth(PersonState.Infected);
    }
}
