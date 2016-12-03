using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerKillCamObsolete : NetworkBehaviour {

	private GameObject[] players;
	private GameObject myself;
	private Camera myCamera;
	public Camera targetCam;
	private GameObject targetPlayer;

	// Use this for initialization
	void Start () 
	{
		myself = this.gameObject;
		myCamera = GetComponentInChildren<Camera> ();
	}

	public void GetOtherPlayerCam()
	{
		if (isLocalPlayer) 
		{
			StartCoroutine (FindAllPlayers ());
			if (targetPlayer != myself || players.Length == 1 ) {

				targetCam = targetPlayer.transform.FindChild("CamCollider").GetComponentInChildren<Camera> ();
				OtherPlayerCamOn ();
			} else 
			{
				GetOtherPlayerCam ();
			}
		}
	}
	public void ResetMyCamera()
	{
		if (isLocalPlayer) 
		{
			targetCam.enabled = false;
			myCamera.enabled = true;
		}
	}
	IEnumerator FindAllPlayers()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		Debug.Log (players.Length);
		yield return new WaitForEndOfFrame ();
		targetPlayer = players[Random.Range(0, players.Length)];
		Debug.Log (targetPlayer);

	}
	void OtherPlayerCamOn ()
	{
		targetCam.gameObject.SetActive(true);
		myCamera.gameObject.SetActive(false);
	}
}
