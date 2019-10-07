using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public  class MasterObserver: MonoBehaviour
{

	private static Sphere[] spheres;
	private Vector3 masterBallPosition;
	private Vector3 slaveBallPosition;
	private Vector3 normalOfPlane;
	private Sphere player;
	private float hitPower = 0;
	private const float MAX_POWER = 50;
	public Text[] textPanes;
	public Text scoreText;
	public Text powerText;
	private double totalAvailableScore;
	private bool enabled;

	public bool shotTaken;

	private Camera mainCamera;

	void Start()
	{

		textPanes = FindObjectsOfType<Text>();
		foreach (Text t in textPanes)
		{
			if (t.text.Contains("Power"))
				powerText = t;
			if (t.text.Contains("Score"))
				scoreText = t;
		}
		spheres = FindObjectsOfType<Sphere>();
		Sphere.sphereList = spheres.ToList();
		mainCamera = FindObjectOfType<Camera>();


		foreach (Sphere sphere in spheres)
		{
			if (sphere.isPlayer)
				player = sphere;
			else
				totalAvailableScore += sphere.mass + sphere.radius;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!enabled)
		{
			foreach (Sphere sphere in spheres)
			{
				if (sphere.isPlayer)
					player = sphere;
				else
					totalAvailableScore += sphere.mass + sphere.radius;
			}

			enabled = true;
		}

		powerText.text = "Power : " + hitPower + " / 50";
		scoreText.text = "Score : " + Math.Round(Pockets.score, 2) + " / " + Math.Round(totalAvailableScore, 2);
		if (Input.GetKey("space") && hitPower < MAX_POWER)
		{
			hitPower += .5f;
		}

		if (Input.GetKey("up"))
			player.transform.position += Vector3.up * Time.deltaTime;
		
		if(Input.GetKey("d"))
			player.transform.localEulerAngles += new Vector3(0, 30f*Time.deltaTime, 0);
		if(Input.GetKey("a"))
			player.transform.localEulerAngles += new Vector3(0, -30f*Time.deltaTime, 0);

		if (Input.GetKeyUp("space"))
		{
			player.velocity = mainCamera.transform.forward * hitPower;
			player.transform.position += player.velocity * Time.deltaTime;
			hitPower = 0;
		}
	}

	public static void updateSpheresList()
	{
		spheres = FindObjectsOfType<Sphere>();
	}
}
