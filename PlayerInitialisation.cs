using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerInitialisation : NetworkBehaviour {
	public GameObject camMenu;
	// Use this for initialization
	void Start () {

		camMenu = GameObject.FindGameObjectWithTag ("MainCamera");

		if (isLocalPlayer) 
			
		{
			gameObject.GetComponent<PlayerController> ().enabled = true;
			gameObject.GetComponentInChildren<Camera> ().enabled = true;
			gameObject.GetComponentInChildren<AudioListener> ().enabled = true;
			camMenu.gameObject.SetActive (false);

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
