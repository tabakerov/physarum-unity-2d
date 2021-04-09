using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public int areaWidth;
    public int areaHeight;

    public float pointSize;
    
    public GameObject point;

    public Point[,] points;
    public GameObject actorPrefab;
    public int numOfActors;

    private void Start()
    {
        Spawn();
        SetNeighboursDumbWay();
        SpawnActors();
    }

    private void SpawnActors()
    {
        for (int s = 0; s < numOfActors; s++)
        {
            var actor = Instantiate(
                            actorPrefab,
                            new Vector3(Random.Range(0f, pointSize * areaWidth),
                                            Random.Range(0f, pointSize * areaHeight),
                                            0f), 
                            Quaternion.identity);
            var actorComponent = actor.GetComponent<Actor>();
            actorComponent.angle = Random.Range(0, 4 * Mathf.PI);
            actorComponent.pointSize = pointSize;
            actorComponent.areaHeight = (areaHeight - 1) * pointSize;
            actorComponent.areaWidth = (areaWidth - 1) * pointSize;
            actorComponent.spawner = this;
        }
    }

    private void SetNeighbours()
    {
        var objects = FindObjectsOfType<Point>();
        Debug.Log($" i have {objects.Length} points");
        foreach (var p in objects)
        {
            var collider = p.gameObject.GetComponent<Collider2D>();
            Debug.Log($"there is a collider {collider.gameObject.name}");
            var colliders = new Collider2D[8];
            
            var n = collider.GetContacts(colliders);
            Debug.Log($"found {n} neighbours");
            foreach (var c in colliders)
            {
                if (c != null)
                {
                    var neighbourPoint = c.gameObject.GetComponent<Point>();
                    if (neighbourPoint != null)
                    {
                        p.points.Add(neighbourPoint);
                    }
                }
            }
        }
    }

    private void SetNeighboursDumbWay()
    {
        for (int a = 0; a < areaWidth; a++)
        {
            for (int b = 0; b < areaHeight; b++)
            {
                if (a > 0 && b > 0) points[a,b].points.Add(points[a-1, b-1]);
                if (a > 0) points[a,b].points.Add(points[a-1, b]);
                if (a > 0 && b < areaHeight - 1) points[a,b].points.Add(points[a-1, b+1]);
                
                if (b > 0) points[a,b].points.Add(points[a, b-1]);
                if (b < areaHeight - 1) points[a, b].points.Add(points[a, b+1]);
                
                if (a < areaWidth - 1) points[a, b].points.Add(points[a+1, b]);
                if (a < areaWidth - 1 && b > 0) points[a, b].points.Add(points[a+1, b-1]);
                if (a < areaWidth - 1 && b < areaHeight - 1) points[a, b].points.Add(points[a+1, b+1]);
            }
        }
    }

    private void Spawn()
    {
        points = new Point[areaWidth, areaHeight];
        
        for (int a = 0; a < areaWidth; a++)
        {
            for (int b = 0; b < areaHeight; b++)
            {
                points[a, b] = Instantiate(point,
                                new Vector3((float) a * pointSize, (float) b * pointSize, 0f),
                                Quaternion.identity).GetComponent<Point>();
                    
            }
        }
    }
}
