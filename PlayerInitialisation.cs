using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerInitialisation : NetworkBehaviour {
	public bool isThePlayer = false;
	
	private GameObject camMenu;

	[SerializeField] Camera playerlocalCam;

	[SerializeField] AudioListener playerAudioL;
	// Use this for initialization

	void Start () {



		if (isLocalPlayer) 
		{
			camMenu = GameObject.Find("Scene Camera");
			gameObject.GetComponent<PlayerController> ().enabled = true;
			playerlocalCam.enabled = true;
			playerAudioL.enabled = true;
			camMenu.gameObject.SetActive (false);
			isThePlayer = true;
	
		}	
	}
}
