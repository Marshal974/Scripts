using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEndOfGameHandler : NetworkBehaviour {
	
	Rigidbody rb;

	void Start()
	{
		rb = this.GetComponent<Rigidbody> ();
	}
	GameObject[] playerList;


	void OnTriggerEnter(Collider gameEnder)
	{
		if (gameEnder.tag == "Finish" && isLocalPlayer) 
		{
			CmdIsAtTheEnd ();
		}
	}

	[Command]
	void CmdIsAtTheEnd ()
	{
		playerList = GameObject.FindGameObjectsWithTag ("Player");

		foreach(GameObject i in playerList)
		{
			i.GetComponent<PlayerEndOfGameHandler> ().RpcGameIsOver ();
		}
	}

	[ClientRpc]
	public void RpcGameIsOver()
	{
		if (isLocalPlayer) 
		{
			gameObject.GetComponent<PlayerController> ().enabled = false;
			gameObject.transform.position = GameObject.Find("EndPoint "+Random.Range(0, 2)).transform.position;
//			rb.useGravity = false;
//			rb.isKinematic = true;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			gameObject.GetComponent<PlayerOnCollision>().PlayerAnnounce.text = "Game is over?! A noob step on the wrong button.Let's say he won...For now!";
			if (gameObject.GetComponent<PlayerOnCollision> ().alive == false) 
			{
				
				gameObject.GetComponent<PlayerKillCam> ().ResetCam ();
				GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = false;
				GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = false;
				gameObject.GetComponent<PlayerOnCollision> ().alive = true;
			}
		}

	}
}