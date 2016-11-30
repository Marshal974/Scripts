using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BadBoyPath : NetworkBehaviour {
	public Transform[] patrolPoints;
	public float speedBB;
	private int currentPoint;

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

			if (transform.position == patrolPoints [currentPoint].transform.position) {
				currentPoint++;
			}

			if (currentPoint >= patrolPoints.Length) {
				currentPoint = 0;
			}

			transform.position = Vector3.MoveTowards (transform.position, patrolPoints [currentPoint].transform.position, speedBB * Time.deltaTime);
	
		}
	}
}
