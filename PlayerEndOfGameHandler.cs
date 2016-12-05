using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEndOfGameHandler : NetworkBehaviour {


}
//
//	public List<GameObject> playerNameShow = new List<GameObject>();
//	public GameObject[] playerRezShow = new GameObject[5];
//	public GameObject[] playerDeathShow = new GameObject[5];
//
//	GameObject[] playerList;
////
////	// Use this for initialization
//	void Start () {
//		if (isLocalPlayer) {
//			playerNameShow.Add( GameObject.Find ("Player1name"));
//			playerNameShow.Add( GameObject.Find ("Player2name")); // aterminer
//		}
//	}
//
//	void OnTriggerEnter(Collider gameEnder)
//	{
//		if (gameEnder.tag == "Finish" && isLocalPlayer) 
//		{
//			CmdIsAtTheEnd ();
//		}
//	}
//
//	[Command]
//	void CmdIsAtTheEnd ()
//	{
//		playerList = GameObject.FindGameObjectsWithTag ("Player");
//
//		foreach(GameObject i in playerList)
//		{
//			i.GetComponent<PlayerEndOfGameHandler> ().RpcGameIsOver ();
//		}
//	}
//
//	[ClientRpc]
//	public void RpcGameIsOver()
//	{
//		if (isLocalPlayer) {
//			gameObject.GetComponent<PlayerController> ().enabled = false;
//
//
//			for (int i = 0; i < playerList.Length; i++) {
//				if (playerList [i] != null) {
//					SetPName (playerList [i].GetComponent<ChoosePlayerName> ().pname);
//
//					SetPDeath (playerList [i].GetComponent<PlayerOnCollision> ().DeathCount.ToString ());
//
//					SetPRez (playerList [i].GetComponent<Player_SavedAGuy> ().rezCount.ToString ());
//				}
//			}
//		}
//	}
//	void SetPName(string otherPname)
//	{
//		//search for an empty textfield in text and put the variable there.
//		for (int i = 0; i < playerNameShow.Count; i++) {
//			if (playerNameShow[i] !=null){
//			if (playerNameShow[i].GetComponent<Text> ().text == "") 
//				{
//				playerNameShow [i].GetComponent<Text> ().text = otherPname;
//					return;
//
//				} 
//
//			}
//		}
//	}
//
//
//	void ChangeStringSpot(string i, GameObject y)
//	{
//		GameObject.Find (y.name).GetComponent<Text> ().text = i;
//		
//	}
//	void SetPDeath(string otherDeathCount)
//	{
//		//search for an empty textfield in text and put the variable there.
//		for (int i = 0; i < playerDeathShow.Length; i++) {
//			if (playerDeathShow [i].GetComponent<Text>().text == "") 
//			{
//				playerDeathShow[i].GetComponent<Text>().text = otherDeathCount;
//				return;
//			}
//
//		}
//	}
//	void SetPRez(string otherRezCount)
//	{
//		//search for an empty textfield in text and put the variable there.
//		for (int i = 0; i < playerRezShow.Length; i++) {
//			foreach (GameObject y in playerRezShow) {
//				if (y.GetComponent<Text>().text == "") 
//				{
//					ChangeStringSpot (otherRezCount, y);
//					return;
//				}
//			}
//		}
//	}

