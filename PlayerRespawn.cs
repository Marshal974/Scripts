using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;


public class PlayerRespawn : NetworkBehaviour {
	public AudioClip respawnSound;

	void Start () {
		
		if (isLocalPlayer) {
			GameObject.Find ("RespawnBtn").GetComponent<Button> ().onClick.AddListener (CallForRez);
		}
	}

	public void CallForRez(){

		if (isLocalPlayer && gameObject.GetComponent<PlayerOnCollision>().alive == false) 
		{
				CmdRespawn ();

		}

	}
	[Command]
	void CmdRespawn (){

		//gameObject.transform.position = GameObject.Find("SpawnPoint 2").transform.position;
		gameObject.GetComponent<PlayerOnCollision> ().RpcOnRez();
		AudioSource.PlayClipAtPoint(respawnSound, gameObject.transform.position);
		RpcRespawn ();

	}
	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			gameObject.transform.position = GameObject.Find("SpawnPoint 2").transform.position;
		}
	}


}
