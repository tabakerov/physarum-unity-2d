using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public float value;

    public List<Point> points;
    public float blurFactor;
    public float decayRate;
    private SpriteRenderer _renderer;
    
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        value = Mathf.Pow(Random.value, 4);
    }

    void Blur()
    {
        var v = value * blurFactor * Time.deltaTime;
        value -= v;
        foreach (var point in points)
        {
            point.value += v / points.Count;
        }

        value /= (1 + Time.deltaTime * decayRate);
    }

    // Update is called once per frame
    void Update()
    {
        Blur();
        _renderer.color = new Color(value, value, value);
    }
}
