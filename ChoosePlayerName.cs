﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChoosePlayerName : NetworkBehaviour {

	[SyncVar(hook = "ChangeMyNameOnOtherP")] public string pname;

	public void SayYourName()
	{
		if (isLocalPlayer && GameObject.Find("NameInputField").transform.FindChild("Text").GetComponent<Text>().text != "" ) 
		{
			StartCoroutine (InputedName());


		}
	}
	IEnumerator InputedName()
	{
		yield return new WaitForFixedUpdate();
		pname = GameObject.Find("NameInputField").transform.FindChild("Text").GetComponent<Text>().text;
		CmdChangeName (pname);
		
	}

	public void ChangeMyNameOnOtherP(string newPname)
	{
		gameObject.GetComponent<PlayerOnCollision> ().OnChangeNickname (newPname);
		pname = newPname;
	}

	[Command]
	public void CmdChangeName(string newName)             //string newName)
	{
		gameObject.GetComponent<PlayerChangeColor> ().ChangeColor ();
	  	pname = newName;
		RpcChangeThatName (newName);

	}
	public override void OnStartLocalPlayer()
	{
		GameObject.Find ("ChangeYourName").GetComponent<Button> ().onClick.AddListener (SayYourName);
	}

	IEnumerator GetNamesOfCoPlayers()
	{
		yield return new WaitForSeconds (4f);
		ChangeMyNameOnOtherP (pname);
	}
	public override void OnStartClient ()
	{
		StartCoroutine (GetNamesOfCoPlayers ());
	}

	[ClientRpc]
	public void RpcChangeThatName (string newName)
	{
		gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer = newName;
		this.GetComponentInChildren<TextMesh> ().text = newName;

	}
}
