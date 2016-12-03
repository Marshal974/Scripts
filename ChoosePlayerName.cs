using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChoosePlayerName : NetworkBehaviour {

	[SyncVar] public string pname = "Player";

	public void SayYourName()
	{
		if (isLocalPlayer && GameObject.Find("NameInputField").transform.FindChild("Text").GetComponent<Text>().text != "" ) 
		{
			pname = GameObject.Find("NameInputField").transform.FindChild("Text").GetComponent<Text>().text;
			CmdChangeName (pname);
		}
	}

	[Command]
	public void CmdChangeName(string newname)             //string newName)
	{
		gameObject.GetComponent<PlayerChangeColor> ().CmdChangeColor ();
		pname = newname;
		RpcChangeThatName (pname);

	}

	void Start () {
		if(isLocalPlayer){
			GameObject.Find ("ChangeYourName").GetComponent<Button> ().onClick.AddListener (SayYourName);
		}
	}
	
	// Update is called once per frame
	void Update () {

		this.GetComponentInChildren<TextMesh> ().text = pname;

	}
	[ClientRpc]
	public void RpcChangeThatName (string newName)
	{
//		if (isLocalPlayer) {
			gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer = newName;
//		}
	}
}
