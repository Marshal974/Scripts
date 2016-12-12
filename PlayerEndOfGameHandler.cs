using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerEndOfGameHandler : NetworkBehaviour {
	
	Rigidbody rb;
	public List<int> endScore = new List<int>();
	public int bestScore = 0;
	public int winnerIndex;
	public string winnerName;
	GameObject messEndGame;
	GameObject victoryImg;

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
		gameObject.GetComponent<PlayerOnCollision> ().wonTheGame = true;

		foreach(GameObject i in playerList)
		{
			i.GetComponent<PlayerEndOfGameHandler> ().RpcGameIsOver ();
			i.GetComponent<PlayerOnCollision> ().RpcFindScoreEndOfGame ();
		}
 
	}

	[ClientRpc]
	public void RpcGameIsOver()
	{
		if (isLocalPlayer) 
		{
			GameObject.Find ("Tabulation").GetComponent<ShowTab> ().IfGameOverShow ();
			gameObject.GetComponent<PlayerController> ().enabled = false;
			gameObject.transform.position = GameObject.Find("EndPoint "+Random.Range(0, 2)).transform.position;
//			rb.useGravity = false;
//			rb.isKinematic = true;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
//			gameObject.GetComponent<PlayerOnCollision>().PlayerAnnounce.text = "Game is over?! A noob step on the wrong button.Let's say he won...For now!";
			if (gameObject.GetComponent<PlayerOnCollision> ().alive == false) 
			{
				
				gameObject.GetComponent<PlayerKillCam> ().ResetCam ();
				GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = false;
				GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = false;
				gameObject.GetComponent<PlayerOnCollision> ().alive = true;

			}

		}
		StartCoroutine(WhoWon());
	}

	IEnumerator WhoWon()
	{
		//prendre chaque nombre ds le tableau des scores... pour chaque nbre vérifié si il est plus grand que tous les autres.
		// si il est plus grand, il dit a tous les joueurs qui a gagné; et il s'affiche pour lui : victoire. les autres défaites ? 
		yield return new WaitForSeconds(4f);

		List<Text> allPScores = gameObject.GetComponent<PlayerOnCollision> ().allPlayersScore;
		int scTmp2;
		foreach ( Text i in allPScores)
		{
			int.TryParse (i.text, out scTmp2);
			endScore.Add( scTmp2);
//			Debug.Log (endScore.ToString());
			yield return new WaitForEndOfFrame ();
			//finir de comparer tout ce bordel; et dire aux joueurs qui a win.
		}
		yield return new WaitForSeconds (2f);
		foreach (int i in endScore) 
		{
			if (i > bestScore) {
				bestScore = i;
			}
//			else 
//			{
//				break;
//			}
		}
		yield return new WaitForSeconds (0.5f);
		for (int j = 0; j < endScore.Count; j++) 
		{
			if (endScore [j] == bestScore) 
			{
				winnerIndex = j;
				Congratulates (j);
			}
		}
	}

	public void Congratulates(int k)
	{
		
		winnerName = gameObject.GetComponent<PlayerOnCollision> ().allPlayersNickname [k].text;
		messEndGame = GameObject.Find ("EndOfGameMessage");
		if (isLocalPlayer) 
		{
				messEndGame.GetComponent<Text>().text = winnerName +" has won with a score of " +bestScore;
			if (winnerName == gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer) 
			{
				GameObject.Find ("VictoryImg").GetComponent<Image> ().enabled = true;
			}
		}
	}
	public override void OnNetworkDestroy ()
	{
		if (!isLocalPlayer) {
			GameObject activeP = gameObject.GetComponent<PlayerOnCollision> ().theActivePlayer;
//		activeP.GetComponent<PlayerOnCollision> ().allPlayersdeath.RemoveAt (winnerIndex);
//		activeP.GetComponent<PlayerOnCollision> ().allPlayersNickname.RemoveAt (winnerIndex);
//			activeP.GetComponent<PlayerOnCollision> ().allPlayersRealNames.RemoveAt (winnerIndex);
			activeP.GetComponent<PlayerOnCollision> ().RemoveThatLeaver (gameObject.name);
//		activeP.GetComponent<PlayerOnCollision> ().allPlayersRez.RemoveAt (winnerIndex);
//		activeP.GetComponent<PlayerOnCollision> ().allPlayersScore.RemoveAt (winnerIndex);
			base.OnNetworkDestroy ();
		}
	}
//	public static int CalculateScore ( int scDeath, int scRez, bool won)
//	{
//		int result;
//		result = ((2*scRez - scDeath) * 10);
//		if (won == false) 
//		{
//			return result;
//		}
//		else
//		{
//			result = result + 50;
//			return result;
//		}
//
//	}

}