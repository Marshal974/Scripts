﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerOnCollision : NetworkBehaviour {
//
//	Vector3 spawnSpot;
	public List<string> allPlayersRealNames = new List<string>();
	public List<Text> allPlayersNickname = new List<Text>();
	public List<Text> allPlayersdeath = new List<Text>();
	public List<Text> allPlayersRez = new List<Text>();
	public List<Text> allPlayersScore = new List<Text>();
	public GameObject NbrDeathOtherPlayer;
	public GameObject theActivePlayer;
	public GameObject deathEffect;
	public int currentScene = 0;
	public Rigidbody rb;
	public GameObject repere;
	public AudioClip[] onBounceSound;
	bool alreadyBouncing = false;
	[SyncVar]public bool alive = true;
	public GameObject lifeEffect;
	private GameObject nbrDeathObj ;
	private Text nbrDeathText;
	private GameObject messObj ;
	public Text PlayerAnnounce;
	[SyncVar(hook = "OnChangeDeath" )]public int DeathCount = 0;
	[SyncVar]public int RezCount = 0;
	public AudioClip onDeathSound;
	public AudioClip onRezSound;
	public string isHeSavior;
	[SyncVar] public string saviorName = "pupute";
	[SyncVar]public string pNameOnPlayer;
	bool isInvincible;
	[SyncVar]public bool wonTheGame;
	string winnerName;
	int scWinTmp;
	string winNameTmp = "";


	ParticleSystem DeadEffect;
	ParticleSystem.EmissionModule DeadEffectEmi;

	void Start () {

		DeadEffect = GetComponent<ParticleSystem> ();
		DeadEffectEmi = DeadEffect.emission;
		pNameOnPlayer = this.GetComponent<ChoosePlayerName> ().pname; 
//		spawnSpot = transform.position;
		rb = this.GetComponent<Rigidbody> ();		
		messObj = GameObject.Find("LocalMessage");
		nbrDeathObj = GameObject.Find ("NbrDeath");
		nbrDeathText = nbrDeathObj.GetComponent<Text> ();
		PlayerAnnounce = messObj.GetComponent<Text> ();
		SetEmiDown ();		

		if (isLocalPlayer) {
			theActivePlayer = gameObject;
			GameObject[] PlayerObjectsArr = GameObject.FindGameObjectsWithTag ("Player");

			foreach (GameObject i in PlayerObjectsArr) {
				i.GetComponent<PlayerOnCollision> ().IAmNew ();
				}

			allPlayersNickname.Add( GameObject.Find ("Player1name").GetComponent<Text>());
			allPlayersNickname.Add( GameObject.Find ("Player2name").GetComponent<Text>());
			allPlayersNickname.Add( GameObject.Find ("Player3name").GetComponent<Text>());
			allPlayersNickname.Add( GameObject.Find ("Player4name").GetComponent<Text>());
			allPlayersNickname.Add( GameObject.Find ("Player5name").GetComponent<Text>());
			allPlayersdeath.Add (GameObject.Find("Player1death").GetComponent<Text>());
			allPlayersdeath.Add (GameObject.Find("Player2death").GetComponent<Text>());
			allPlayersdeath.Add (GameObject.Find("Player3death").GetComponent<Text>());
			allPlayersdeath.Add (GameObject.Find("Player4death").GetComponent<Text>());
			allPlayersdeath.Add (GameObject.Find("Player5death").GetComponent<Text>());
			allPlayersRez.Add (GameObject.Find("Player1rez").GetComponent<Text>());
			allPlayersRez.Add (GameObject.Find("Player2rez").GetComponent<Text>());
			allPlayersRez.Add (GameObject.Find("Player3rez").GetComponent<Text>());
			allPlayersRez.Add (GameObject.Find("Player4rez").GetComponent<Text>());
			allPlayersRez.Add (GameObject.Find("Player5rez").GetComponent<Text>());
			allPlayersScore.Add (GameObject.Find("Player1score").GetComponent<Text>());
			allPlayersScore.Add (GameObject.Find("Player2score").GetComponent<Text>());
			allPlayersScore.Add (GameObject.Find("Player3score").GetComponent<Text>());
			allPlayersScore.Add (GameObject.Find("Player4score").GetComponent<Text>());
			allPlayersScore.Add (GameObject.Find("Player5score").GetComponent<Text>());



		}
		if (!isLocalPlayer) {


			GameObject[] PlayerObjectsArr = GameObject.FindGameObjectsWithTag ("Player");

			foreach (GameObject i in PlayerObjectsArr) {
				if (i.GetComponent<PlayerInitialisation> ().isThePlayer == true) {
					theActivePlayer = i;
					break;
				}
			}
			InitializeMe ();


		}

	}
//	[Command]
	public void IAmNew ()
	{
		InitializeMe ();
	}
	[Command]
	public void CmdRegisterIndex()
	{
		gameObject.GetComponent<PlayerEndOfGameHandler> ().winnerIndex = allPlayersRealNames.Count;
	}

//	[ClientRpc]
	public void InitializeMe()
	{
//		if (!isLocalPlayer) {
		GameObject[] PlayerObjectsArr = GameObject.FindGameObjectsWithTag ("Player");

			 foreach (GameObject i in PlayerObjectsArr) {
			if (i.GetComponent<PlayerInitialisation> ().isThePlayer == true) {
				theActivePlayer = i;
//				return;
//			}
		}


			StartCoroutine (DontSpeed ());

		}
	}
	IEnumerator DontSpeed()
	{
		yield return new WaitForSeconds (0.5f);
		theActivePlayer.GetComponent<PlayerOnCollision> ().AddPlayerRN (gameObject.name);


	}
	void SetEmiUp()
	{
		DeadEffectEmi.rate = new ParticleSystem.MinMaxCurve (10f);
	}
	void SetEmiDown()
	{
		DeadEffectEmi.rate = new ParticleSystem.MinMaxCurve (0f);
	}
		
	void OnCollisionEnter (Collision other) 
	{
		if (!isLocalPlayer) 
		{
			return;
		}
		if (other.gameObject.tag == "BadGuys" && alive == true && isInvincible == false) 
		{
				CmdIAmDead ();
		}
		if (other.gameObject.tag == "Bounce" && alreadyBouncing == false) 
			{
				StartCoroutine (RndBounce ());
				alreadyBouncing = true;
			}
	}


	IEnumerator RndBounce()
	{
		AudioSource.PlayClipAtPoint (onBounceSound[Random.Range(0, onBounceSound.Length)], gameObject.transform.position);
		yield return new WaitForSeconds (5f);
		alreadyBouncing = false;
	}


	void OnTriggerExit(Collider otherC)
	{
		if (!isLocalPlayer) 
		{
			return;
		}
		if (alive == false) {
			if (otherC.gameObject.tag == "Player" ) 
			{
				saviorName = otherC.gameObject.GetComponent<PlayerOnCollision> ().pNameOnPlayer;
				alive = true; //recent. evite le double point gagner? 
				CmdTellThemAll (saviorName, pNameOnPlayer);
			}
		}
	}
	[Command]
	public void CmdIAmDead()
	{
		RpcOnDeath ();
	}
		
	[ClientRpc]
	public void RpcOnDeath()
	{
		Instantiate (deathEffect, transform.position, Quaternion.identity);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		AudioSource.PlayClipAtPoint (onDeathSound, gameObject.transform.position);
		DeathCount++;
		alive = false;
		rb.useGravity = false;
		rb.isKinematic = true;
		gameObject.GetComponent<BoxCollider> ().isTrigger = true; 
		if (!isLocalPlayer) {
			SetEmiUp ();

		}
		if (isLocalPlayer) {
			gameObject.GetComponent<PlayerController> ().enabled = false;
			GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = true;
			GameObject.Find ("BgKillcam").GetComponent<Image> ().enabled = true;
			GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = true;
			nbrDeathText.text = DeathCount.ToString ();
			gameObject.GetComponent<PlayerKillCam> ().FindNewTarget ();
		}
	}

	[ClientRpc]
	public void RpcOnRez()
	{
		if (isLocalPlayer) {

			gameObject.GetComponent<PlayerController> ().enabled = true;
			GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = false;
			GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = false;
			GameObject.Find ("BgKillcam").GetComponent<Image> ().enabled = false;
			gameObject.GetComponent<PlayerKillCam> ().ResetCam ();
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			StartCoroutine (Invincible (1.5f));
		}
			
		Instantiate (lifeEffect, transform.position, Quaternion.identity);
		gameObject.GetComponent<BoxCollider> ().isTrigger = false;
		alive = true;
		rb.isKinematic = false;
		AudioSource.PlayClipAtPoint (onRezSound, gameObject.transform.position);
		rb.useGravity = true;
		if (!isLocalPlayer) {
//			alive = true;

			SetEmiDown ();
		}
	}
	IEnumerator Invincible(float inv)
	{
		isInvincible = true;
		yield return new WaitForSeconds (inv);
		isInvincible = false;
	}

	IEnumerator ClientLocalMessage(string otherCName){
		string totalMess;
		totalMess = otherCName + " just saved your ass! Run kitty Run!";
		PlayerAnnounce.text = totalMess;
		yield return new WaitForSeconds (3.0f);
		PlayerAnnounce.text = "";
	}

	IEnumerator OtherClientLocalMessage(string otherCName, string elseCname){
		PlayerAnnounce.text = otherCName + " just saved " + elseCname + ". Well Done !";
		yield return new WaitForSeconds (3.0f);
		PlayerAnnounce.text = "";
	}

	[Command]
	void CmdTellThemAll(string savior, string saved){
		RpcShowRespects (savior, saved);
		RpcOnRez ();
	}
	[ClientRpc]

	void RpcShowRespects (string savior2, string saved2)
	{


		if (isLocalPlayer) {
			StartCoroutine (ClientLocalMessage (savior2));

		} else {
			StartCoroutine (OtherClientLocalMessage (savior2, saved2));

		}
	

	}
	public void OnChangeDeath(int deaths){

//		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherDeaths (deaths, gameObject.name);

//		}

	}

	public void OnChangeNickname(string nickN){

//		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherNickname (nickN, gameObject.name);
//		}
	}

	public void AddPlayerRN (string playerRN){
		foreach (string i in allPlayersRealNames) 
		{
			if (i == playerRN) {
				
				return;
			}
			
		}
		allPlayersRealNames.Add (playerRN);




	}

	public void ChangeOtherDeaths(int death, string Rname)
	{
		StartCoroutine (ChangeThatDeathTab (death, Rname));
	}

	public void ChangeOtherRez(int rezs, string Rname)
	{
		StartCoroutine (ChangeThatRezTab(rezs, Rname));
	}
	public void RemoveThatLeaver(string rname)
	{
		for (int l = 0; l<allPlayersRealNames.Count;l++)
		{
			if (rname == allPlayersRealNames [l]) 
			{
				allPlayersRealNames.RemoveAt (l);
				return;
			}
		}
	}
	IEnumerator ChangeThatRezTab(int rezs3, string Rname3){

		int rezIndex = GetIndexFromNickname(Rname3);
		yield return new WaitForSeconds (0.2f);
		allPlayersRez [rezIndex].GetComponent<Text>().text = rezs3.ToString ();
	}
	IEnumerator ChangeThatDeathTab(int deaths3, string Rname4){

		int deathIndex = GetIndexFromNickname(Rname4);
		yield return new WaitForSeconds (0.2f);
		allPlayersdeath [deathIndex].GetComponent<Text>().text = deaths3.ToString ();
	}


	public int GetIndexFromNickname(string Rname2)
	{


		for (int i = 0; i < allPlayersRealNames.Count; i++) 
		{
			if (allPlayersRealNames [i] == Rname2) {

				return i;
			} 
		}
		return 5;
	}

	public void ChangeOtherNickname(string Pnamedude, string Rname)
	{
		int deathsIndex = 0;

		for (int i = 0; i < allPlayersRealNames.Count; i++) 
		{
			if (allPlayersRealNames [i] == Rname) {
				deathsIndex = i;
				break;
			}
		}

		allPlayersNickname [deathsIndex].GetComponent<Text>().text = Pnamedude;
	}

	[ClientRpc]
	public void RpcFindScoreEndOfGame()
	{
		if (isLocalPlayer) {
			foreach (Text i in allPlayersScore) 
			{
				for (int j = 0; j < allPlayersRealNames.Count; j++) 
				{
					string deathStrTmp;
					string rezStrTmp;
					int deathTemp;
					int rezTemp;
					int scTemp;
					deathStrTmp = allPlayersdeath [j].text.ToString ();
					rezStrTmp = allPlayersRez [j].text.ToString ();
					int.TryParse (deathStrTmp, out deathTemp);
					int.TryParse (rezStrTmp, out rezTemp);

					scTemp = ((rezTemp * 2) - deathTemp) * 10;
				
					allPlayersScore [j].text = scTemp.ToString ();

				} 
			}
			StartCoroutine(WinnerBonus());
		}
	}

			IEnumerator WinnerBonus()
			{
			GameObject[] PlayerObjectsArr = GameObject.FindGameObjectsWithTag ("Player");

		yield return new WaitForSeconds(0.5f);

			foreach (GameObject i in PlayerObjectsArr) {
				if (i.GetComponent<PlayerOnCollision> ().wonTheGame == true) {
					winnerName = i.name;
					break;
				}
			}
				yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < allPlayersRealNames.Count; i++) 
			{
				if (allPlayersRealNames [i] == winnerName) 
				{
					winNameTmp = allPlayersScore [i].text.ToString ();
					int.TryParse (winNameTmp, out scWinTmp);
					scWinTmp = scWinTmp + 50;
					allPlayersScore [i].text = scWinTmp.ToString ();
					break;
				}
			}
		}

	}



