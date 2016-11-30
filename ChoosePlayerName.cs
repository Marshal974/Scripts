using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ChoosePlayerName : NetworkBehaviour {

	[SyncVar]
	public string pname = "Player";

	void OnGUI(){
		
		if (isLocalPlayer) 
		{
			
			pname = GUI.TextField (new Rect (25, Screen.height - 40, 100, 30), pname);

			if (GUI.Button (new Rect (130, Screen.height - 40, 80, 30), "Change")) 
			{
				CmdChangeName (pname);
			}
		}
			
	}

	[Command]
	public void CmdChangeName(string newName)
	{
		pname = newName;

		RpcChangeThatName (pname);

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.GetComponentInChildren<TextMesh> ().text = pname;

	}
	[ClientRpc]
	void RpcChangeThatName (string newName)
	{
		gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer = newName;
	}
}
