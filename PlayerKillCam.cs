using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerKillCam : NetworkBehaviour {

	Camera myCam;
	public bool activatePlayerFollow;
	GameObject playerToFollow;
	GameObject[] listOfPlayersToFollow;


	void Start () 
	{
		playerToFollow = gameObject;
		myCam = gameObject.GetComponentInChildren<Camera>();
	}

	// va falloir revoir ca c'est con de le faire si souvent....:/
	void FixedUpdate () 
	{
		if (isLocalPlayer) 
		{
			listOfPlayersToFollow = GameObject.FindGameObjectsWithTag ("Player");
		}
	}
		void LateUpdate()
	{

		if (activatePlayerFollow != true) 
		{
			myCam.transform.position = gameObject.transform.FindChild ("CamCollider").transform.position;
			myCam.transform.rotation = gameObject.transform.FindChild ("CamCollider").transform.rotation;
			return;
		}
		if (playerToFollow != gameObject) 
		{
				myCam.transform.position = playerToFollow.transform.FindChild ("CamCollider").transform.position;
				myCam.transform.rotation = playerToFollow.transform.FindChild ("CamCollider").transform.rotation;

//			myCam.transform.position = playerToFollow.transform.position - offSetCam;
//			myCam.transform.LookAt (playerToFollow.transform);
		}

		if (Input.GetMouseButtonDown (1)) 
		{
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
