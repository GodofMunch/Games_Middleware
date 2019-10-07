using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sphere : MonoBehaviour
{
	private Vector3 point;
	private Vector3 normal;
	public float mass;
	public float radius;
	private float randomDiameter;
	public float coefficientOfRestitution;
	private Vector3 acceleration;
	public static List<Sphere> sphereList;
	private Sphere collidedWith;
	private Plane[] planes;
	private const float GRAVITY = 9.88f;
	public Vector3 velocity; 
	public bool isPlayer;

	void Start()
	{
		
		planes = FindObjectsOfType<Plane>();
		mass = Random.Range(.5f, 2f);
		coefficientOfRestitution = Random.Range(.5f, .88f);
		randomDiameter = mass * Random.Range(1f, 2f);
		acceleration = Vector3.down * GRAVITY;
		transform.localScale = new Vector3(randomDiameter, randomDiameter, randomDiameter);
		radius = randomDiameter / 2;
	}

	void Update()
	{
		velocity += acceleration * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;

		foreach (Plane plane in planes)
		{

			Vector3 fromPlaneToSphere = transform.position - plane.transform.position;
			Vector3 planeNormal = plane.normal;


			if (parallel(fromPlaneToSphere, planeNormal).magnitude < radius)
			{
				velocity = collideWithPlane(plane);
			}
		}

		
		foreach (Sphere otherSphere in sphereList)
		{
			if (otherSphere == null)
			{
				sphereList.Remove(otherSphere);
				MasterObserver.updateSpheresList();
			}

			else if (otherSphere != this)
			{
				Vector3 fromThisToOtherSphere = transform.position - otherSphere.transform.position;

				if (fromThisToOtherSphere.magnitude < radius + otherSphere.radius)
					collideWithSphere(otherSphere);
			}
		}

		float score;
		velocity -= velocity * 0.01f;
	}

	private Vector3 collideWithPlane(Plane plane)
	{
		Vector3 parallelToSurface = plane.parallelToSurface(velocity);
		Vector3 perpendicularToSurface = plane.perpendicularToSurface(velocity);
		while(plane.distanceTo(transform.position)<radius)
			transform.position -= perpendicularToSurface * Time.deltaTime;
		transform.position -= perpendicularToSurface * Time.deltaTime;
		return parallelToSurface - perpendicularToSurface * coefficientOfRestitution;
	}
	private Vector3 parallel(Vector3 v, Vector3 n)
	{
		Vector3 norm = n.normalized;
		return Vector3.Dot(v, norm) * norm;
	}

	private Vector3 perpendicular(Vector3 v, Vector3 n)
	{
		return -parallel(v, n);
	}

	private float distanceTo(Sphere otherSphere)
	{
		return (transform.position - otherSphere.transform.position).magnitude;
	}

	private void collideWithSphere(Sphere otherSphere)
	{
		Vector3 n = (transform.position - otherSphere.transform.position).normalized;
		transform.position -= velocity * Time.deltaTime;
		otherSphere.transform.position -= otherSphere.velocity * Time.deltaTime;

		Vector3 thisPerp = perpendicular(velocity, n);
		Vector3 otherPerp = perpendicular(otherSphere.velocity, n);

		Vector3 u1 = parallel(velocity, n);
		Vector3 u2 = parallel(otherSphere.velocity, n);

		float m1 = mass, m2 = otherSphere.mass;

		Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + (2 * m2 / (m1 + m2)) * u2;
		Vector3 v2 = ((m2 - m1) / (m1 + m2)) * u2 + (2 * m1 / (m1 + m2)) * u1;

		velocity = thisPerp + (v1 * coefficientOfRestitution);
		transform.position += velocity * Time.deltaTime;
		otherSphere.velocity = otherPerp + (v2 * coefficientOfRestitution);
		otherSphere.transform.position += otherSphere.velocity * Time.deltaTime;
	}
}
