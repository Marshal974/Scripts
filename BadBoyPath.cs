﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BadBoyPath : NetworkBehaviour {
	public Transform[] patrolPoints;
	public float speedBB;
	[SyncVar(hook = "OnChangeWaypoint")]int currentPoint;
	[SerializeField] bool rndMove;

	// Use this for initialization
	void Start () {
		if (isServer) {
			transform.position = patrolPoints [0].transform.position;
			currentPoint = 0;
		}
	
	}

	// Update is called once per frame
	void Update () {
		if (isServer) {

			if (rndMove == false) {

				if (transform.position == patrolPoints [currentPoint].transform.position) {
					currentPoint++;
				}

				if (currentPoint >= patrolPoints.Length) {
					currentPoint = 0;
				}
			} else 
			{	
				if (transform.position == patrolPoints [currentPoint].transform.position) {
					currentPoint = Random.Range(0, patrolPoints.Length);
				}

			}
		}
			transform.position = Vector3.MoveTowards (transform.position, patrolPoints [currentPoint].transform.position, speedBB * Time.deltaTime);
			transform.rotation = Quaternion.LookRotation(patrolPoints[currentPoint].transform.position);

		}
	

	public void OnChangeWaypoint(int newWayPoint)
	{
		currentPoint = newWayPoint;

	}
}
