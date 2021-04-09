using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public float speed;

    public float angle;

    public float trailRate;

    public float areaWidth;
    public float areaHeight;
    public int numWidth, numHeight;

    public float pointSize;
    public Spawner spawner;

    public float sensorDistance;
    public float sensorAngle;
    public float rotateSpeed;
    
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        Move();
        LeaveTrail();
        Sense();
    }

    void Sense()
    {
        float l, r, c;
        // LEFT
        var x = transform.position.x + pointSize * sensorDistance * Mathf.Cos(angle + sensorAngle);
        var y = transform.position.y + pointSize * sensorDistance * Mathf.Sin(angle + sensorAngle) ;
        Normalize(ref x, ref y);
        int a = Mathf.CeilToInt(x / pointSize);
        int b = Mathf.CeilToInt(y / pointSize);
        l = spawner.points[a, b].value;
        // RIGHT
        x = transform.position.x + pointSize * sensorDistance * Mathf.Cos(angle-sensorAngle);
        y = transform.position.y + pointSize * sensorDistance * Mathf.Sin(angle-sensorAngle);
        Normalize(ref x, ref y);
        a = Mathf.CeilToInt(x / pointSize);
        b = Mathf.CeilToInt(y / pointSize);
        r = spawner.points[a, b].value;
        // CENTER
        x = transform.position.x + pointSize * sensorDistance * Mathf.Cos(angle);
        y = transform.position.y + pointSize * sensorDistance * Mathf.Sin(angle);
        Normalize(ref x, ref y);
        a = Mathf.CeilToInt(x / pointSize);
        b = Mathf.CeilToInt(y / pointSize);
        c = spawner.points[a, b].value;
        if (l > c && l > r)
        {
            angle += rotateSpeed * Time.deltaTime;
        }
        if (r > c && r > l)
        {
            angle -= rotateSpeed * Time.deltaTime;
        }
    }

    void LeaveTrail()
    {
        int x = Mathf.CeilToInt(transform.position.x / pointSize);
        int y = Mathf.CeilToInt(transform.position.y / pointSize);
        spawner.points[x, y].value += trailRate * Time.deltaTime;
    }

    void Normalize(ref float x, ref float y)
    {
        if (x < 0) x += areaWidth;
        if (y < 0) y += areaHeight;
        if (x > areaWidth) x -= areaWidth;
        if (y > areaHeight) y -= areaHeight;
    }

    void Move()
    {
        float x = transform.position.x + Time.deltaTime * speed * Mathf.Cos(angle);
        float y = transform.position.y + Time.deltaTime * speed * Mathf.Sin(angle);
        Normalize(ref x, ref y);

        transform.SetPositionAndRotation(new Vector3(x, y, 0f), Quaternion.identity);
    }
}
