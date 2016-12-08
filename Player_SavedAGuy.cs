using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player_SavedAGuy : NetworkBehaviour {

	[SyncVar(hook = "OnChangeRez")]public int rezCount;
	private GameObject rezMessObj;
	private Text RezMess;
	private bool savedPlayerAlive;
	private bool noMorePts;

	void Start()
	{
		rezMessObj = GameObject.Find ("NbrRez");
		RezMess = rezMessObj.GetComponent<Text> ();
	}

	public void OnTriggerExit (Collider SavedPlayer)
	{

		if (isLocalPlayer && SavedPlayer.gameObject.tag == "Player") {
			savedPlayerAlive = SavedPlayer.gameObject.GetComponent<PlayerOnCollision> ().alive;

			if (savedPlayerAlive == false) {
				CmdISavedAGuy ();
			}
		}
	}


	[Command]
void CmdISavedAGuy()
	{
		if (noMorePts == true) {
			return;
		} else 
		{
			StartCoroutine (NoMorePoints ());
		}
	}

	IEnumerator NoMorePoints()
	{
		rezCount++;
		gameObject.GetComponent<PlayerOnCollision> ().RezCount++;
		noMorePts = true;
		yield return new WaitForSeconds (3);
		noMorePts = false;
	}

//[ClientRpc]
//void RpcGetRezPoint()
//	{
//	if (isLocalPlayer) 
//		{
//
////		rezCount++;
//
//	}

//}
	public void OnChangeRez(int rez){
		rezCount = rez;
		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			gameObject.GetComponent<PlayerOnCollision>().theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherRez (rez, gameObject.name);

		}
		if (isLocalPlayer) 
		{
			RezMess.text = rezCount.ToString();
		}
	}
}