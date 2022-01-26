using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfDetection : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    public Transform[] Points
    {
        get => points;
    }


    public void GetClosestPoint(Vector2 from, out float closestDist, out Transform closestPoint)
    {
        closestDist = 0;
        closestPoint = null;

        float currentClosestDist = Mathf.Infinity;
        float currentDist;

        foreach(Transform point in points)
        {
            currentDist = Vector2.Distance(point.position, from);
            if (currentDist < currentClosestDist)
            {
                currentClosestDist = currentDist;
                closestDist = currentClosestDist;
                closestPoint = point;
            }
        }

    }

    public Transform GetClosestPoint(Vector2 from)
    {
        Transform closestPoint = null;
        float currentClosestDist = Mathf.Infinity;
        float currentDist = 0;

        foreach (Transform point in points)
        {
            currentDist = Vector2.Distance(point.position, from);
            if (currentDist < currentClosestDist)
            {
                currentClosestDist = currentDist;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    public float GetClosestDist(Vector2 from)
    {
        float currentClosestDist = Mathf.Infinity;
        float currentDist = 0;

        foreach (Transform point in points)
        {
            currentDist = Vector2.Distance(point.position, from);
            if (currentDist < currentClosestDist)
            {
                currentClosestDist = currentDist;
            }
        }

        return currentClosestDist;
    }

    public Transform GetMain()
    {
        return points[0];
    }
}