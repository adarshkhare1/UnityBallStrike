using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private int _populationSize = 200;
    private Camera _camera;
    [SerializeField]
    GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        float clampLimit = _camera.orthographicSize - this.transform.localScale.y / 2;
        for (int i = 1; i <= _populationSize; i++)
        {
            float x = Random.Range(-clampLimit, +clampLimit);
            float y = Random.Range(-clampLimit, +clampLimit);
            GameObject b = Instantiate(ballPrefab, new Vector3(x, y), Quaternion.identity);
            b.GetComponent<Ball>().SetHealth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
