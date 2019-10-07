using System;
using UnityEngine;

public class Pockets : MonoBehaviour
{
	public static double score = 0;

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("NonPlayerBall"))
		{
			Sphere ball = collision.GetComponentInParent<Sphere>();
			score += Math.Round(ball.mass, 2) + Math.Round(ball.radius, 2);
			MasterObserver.updateSpheresList();
			Destroy(collision.gameObject);
		}
	}
}
