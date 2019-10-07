using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Vector3 velocity;
    public float radius;
    public Vector3 acceleration;
    private const float COEFFICIENT_OF_RESTITUTION = .8f;
    private Plane[] planes;
    private const float GRAVITY = 9.88f;
    public bool isMaster;
    public float mass;
    private float timeJustBeforeImpact = 0;

    private void Start()
    {
        planes = FindObjectsOfType<Plane>();
        acceleration = Vector3.down * GRAVITY;
        radius = transform.localScale.y / 2;
        
    }

    private void Update()
    {
       
        velocity += acceleration * Time.deltaTime;
        this.transform.position += (COEFFICIENT_OF_RESTITUTION * Time.deltaTime) * velocity;
        

        Plane plane = planes[0];

        for (int i = 0; i < planes.Length; i++)
        {
            if (planes[i].distanceTo(transform.position) < radius)
                plane = planes[i];
        }

        if (plane.distanceTo(transform.position) < radius)
        {
            float timeJustAfterImpact = Time.deltaTime;
            float timeOfImpact = timeJustAfterImpact - timeJustBeforeImpact;
            
            //velocity = moveByTimeOfImpact(timeOfImpact, plane);
            velocity = moveByNormal(plane);
            //velocity = moveBackToPoint(plane);
        }
        timeJustBeforeImpact = Time.deltaTime;
        
        
    }

    private Vector3 moveByNormal(Plane plane)    {
        Vector3 parallelToSurface = plane.parallelToSurface(velocity);
        Vector3 perpendicularToSurface = plane.perpendicularToSurface(velocity);
        while(plane.distanceTo(transform.position)<radius)
            transform.position -= perpendicularToSurface * Time.deltaTime;
        return parallelToSurface - perpendicularToSurface * COEFFICIENT_OF_RESTITUTION;
    }

    private Vector3 moveBackToPoint(Plane plane)
    {
        transform.position -= velocity * Time.deltaTime;
        Vector3 parallelToSurface = plane.parallelToSurface(velocity);
        Vector3 perpendicularToSurface = plane.perpendicularToSurface(velocity);
        return parallelToSurface - perpendicularToSurface * COEFFICIENT_OF_RESTITUTION;
    }

    private Vector3 moveByTimeOfImpact(float toi, Plane plane)
    {
        Vector3 parallelToSurface = plane.parallelToSurface(velocity);
        Vector3 perpendicularToSurface = plane.perpendicularToSurface(velocity);
        transform.position += (parallelToSurface - perpendicularToSurface) * toi;
        return parallelToSurface - perpendicularToSurface * COEFFICIENT_OF_RESTITUTION;
    }

    public float getCoefficientOfRes()
    {
        return COEFFICIENT_OF_RESTITUTION;
    }
}
