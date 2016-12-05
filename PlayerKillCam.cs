using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerKillCam : NetworkBehaviour {

	Camera myCam;
	public bool activatePlayerFollow;
	GameObject playerToFollow;
	GameObject previousPlayerToFollow;
	GameObject[] listOfPlayersToFollow;


	void Start () {
		playerToFollow = gameObject;
		myCam = gameObject.GetComponentInChildren<Camera>();

	
	}

	void FixedUpdate () {
		if (isLocalPlayer) {
			listOfPlayersToFollow = GameObject.FindGameObjectsWithTag ("Player");

		}
	}
		void LateUpdate(){
		if (playerToFollow == gameObject && activatePlayerFollow == true || previousPlayerToFollow == playerToFollow && activatePlayerFollow == true && listOfPlayersToFollow.Length >3) 
		{
			FindNewTarget ();
		} 

		if (playerToFollow != gameObject && activatePlayerFollow == true) 
		{
			myCam.transform.position = playerToFollow.transform.FindChild ("CamCollider").transform.position;
			myCam.transform.rotation = playerToFollow.transform.FindChild ("CamCollider").transform.rotation;
			previousPlayerToFollow = playerToFollow;
//			myCam.transform.position = playerToFollow.transform.position - offSetCam;
//			myCam.transform.LookAt (playerToFollow.transform);
		} else 
		{
			myCam.transform.position = gameObject.transform.FindChild ("CamCollider").transform.position;
			myCam.transform.rotation = gameObject.transform.FindChild ("CamCollider").transform.rotation;
		}

		if (Input.GetMouseButtonDown(1) && activatePlayerFollow == true) {
			FindNewTarget ();
		}

	
	}

	public void FindNewTarget ()
	{
		activatePlayerFollow = true;
		playerToFollow = listOfPlayersToFollow [Random.Range (0, listOfPlayersToFollow.Length)];


	}

	public void ResetCam ()
	{
		activatePlayerFollow = false;
		playerToFollow = gameObject;
	}
	
}
