using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_SavedAGuy : NetworkBehaviour {

	[SyncVar]public int rezCount;
	private GameObject rezMessObj;
	private Text RezMess;
	private bool savedPlayerAlive;

	void Start(){
		rezMessObj = GameObject.Find ("NbrRez");
		RezMess = rezMessObj.GetComponent<Text> ();
	}

	public void OnTriggerEnter(Collider SavedPlayer){

		if (isLocalPlayer && SavedPlayer.gameObject.tag == "Player") {
			savedPlayerAlive = SavedPlayer.gameObject.GetComponent<PlayerOnCollision> ().alive;

			if (savedPlayerAlive == false) {
				CmdISavedAGuy ();
			}
		}
	}


	[Command]
void CmdISavedAGuy(){

		RpcGetRezPoint ();

	}

[ClientRpc]
void RpcGetRezPoint(){
	if (isLocalPlayer) {
		gameObject.GetComponent<PlayerOnCollision> ().RezCount++;
		rezCount++;
		RezMess.text = rezCount.ToString();
	}

}
}