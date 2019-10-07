using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {

    public Vector3 point;
    public Vector3 normal;

    private void Start()
    {
        point = this.transform.position;
        normal = transform.up;
        
    }

    private Vector3 parallel(Vector3 v, Vector3 n)
    {
        //n Should be normalised

        normal = n.normalized;
        return Vector3.Dot(v, normal) * normal;
    }

    private Vector3 perpendicular(Vector3 v, Vector3 n)
    {
        return v -parallel(v, n);
    }
	
    public float distanceTo(Vector3 s)
    {
        return parallel((point - s), normal).magnitude;
    }

    internal Vector3 perpendicularToSurface(Vector3 v)
    {
        return parallel(v, normal);
    }

    internal Vector3 parallelToSurface(Vector3 v)
    {
        return perpendicular(v, normal);
    }
}
