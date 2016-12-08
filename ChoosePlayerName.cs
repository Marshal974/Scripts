using UnityEngine;
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
//		yield return new WaitForFixedUpdate();
		CmdChangeName (pname);
		
	}

	public void ChangeMyNameOnOtherP(string newPname)
	{
		if (!isLocalPlayer) {
			gameObject.GetComponent<PlayerOnCollision> ().OnChangeNickname (newPname);
	
		}
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
//	void Start () {
//		if(!isLocalPlayer)
//		{
//
//		}
//	}
	IEnumerator GetNamesOfCoPlayers()
	{
		yield return new WaitForSeconds (2f);
		ChangeMyNameOnOtherP (pname);
	}
	public override void OnStartClient ()
	{
		StartCoroutine (GetNamesOfCoPlayers ());
	}

	[ClientRpc]
	public void RpcChangeThatName (string newName)
	{
//		if (isLocalPlayer) {
			gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer = newName;
		this.GetComponentInChildren<TextMesh> ().text = newName;

//		pname = newName;

//		}
	}
}
