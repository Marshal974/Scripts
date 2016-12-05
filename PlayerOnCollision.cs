using UnityEngine;
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
	public GameObject NbrDeathOtherPlayer;
	public GameObject theActivePlayer;
	public GameObject deathEffect;
	public int currentScene = 0;
	public Rigidbody rb;
	public GameObject repere;
	public AudioClip onBounceSound;

	[SyncVar]public bool alive = true;
	public GameObject lifeEffect;
	private GameObject nbrDeathObj ;
	private Text nbrDeathText;
	private GameObject messObj ;
	public Text PlayerAnnounce;
	[SyncVar(hook = "OnChangeDeath" )]public int DeathCount = 0;
	[SyncVar(hook = "OnChangeRez")]public int RezCount = 0;
	public AudioClip onDeathSound;
	public AudioClip onRezSound;
	public string isHeSavior;
	[SyncVar]
	public string saviorName = "pupute";


	[SyncVar]public string pNameOnPlayer;

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

//	[ClientRpc]
	public void InitializeMe()
	{
		if (!isLocalPlayer) {
		GameObject[] PlayerObjectsArr = GameObject.FindGameObjectsWithTag ("Player");

			 foreach (GameObject i in PlayerObjectsArr) {
			if (i.GetComponent<PlayerInitialisation> ().isThePlayer == true) {
				theActivePlayer = i;
//				return;
			}
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
		
	void OnCollisionEnter (Collision other) {

		if (other.gameObject.tag == "BadGuys" && alive == true && isLocalPlayer) {
			CmdIAmDead ();
		}
				
	}
	[Command]
	public void CmdIAmDead(){
		RpcOnDeath ();
	}
		void OnTriggerEnter(Collider otherC)
	{

		if (isLocalPlayer) {
			if (otherC.gameObject.tag == "Player" && alive == false) {
				saviorName = otherC.gameObject.GetComponent<PlayerOnCollision>().pNameOnPlayer;
			CmdTellThemAll (saviorName, pNameOnPlayer);


			}
		}
	}
		
	[ClientRpc]
	public void RpcOnDeath()
	{
		Instantiate (deathEffect, transform.position, Quaternion.identity);
		gameObject.GetComponent<BoxCollider> ().isTrigger = true; 
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		AudioSource.PlayClipAtPoint (onDeathSound, gameObject.transform.position);
		DeathCount++;
		alive = false;
		rb.useGravity = false;
		rb.isKinematic = true;
		if (!isLocalPlayer) {
			SetEmiUp ();

		}
		if (isLocalPlayer) {
			GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = true;
			GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = true;
			gameObject.GetComponent<PlayerController> ().enabled = false;

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
			gameObject.GetComponent<PlayerKillCam> ().ResetCam ();
		}


		Instantiate (lifeEffect, transform.position, Quaternion.identity);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.isKinematic = false;
		AudioSource.PlayClipAtPoint (onRezSound, gameObject.transform.position);
		alive = true;
		gameObject.GetComponent<BoxCollider> ().isTrigger = false; 
		rb.useGravity = true;
		if (!isLocalPlayer) {
			SetEmiDown ();
		}
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

		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherDeaths (deaths, gameObject.name);
		}
	}
	public void OnChangeRez(int rez){

		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherRez (rez, gameObject.name);
		}
	}
	public void OnChangeNickname(string nickN){

		if(!isLocalPlayer){
			//il faut lui dire qui tu es (pnameonplayer) et le nouveau score de morts (deaths)
			//ensuite il recoit la commande qui va le faire rechercher la ligne correspondant a ton joueur; de la il en déduira la case correcpondant a ta mort; et prendra le deaths pour le mettre dedans.
			//il doit donc déja savoir quelle ligne est la tienne
			//quand tu te co, si t pas localtoutca, mais que t'es sur un client; tu dois lui dire : je suis nouveau, attribue moi une ligne dans ta liste; met dans la premiere case mon nom; apres mon scoredeath; et mon scorerez; et souviens toi que tout ca c est a moi.

			theActivePlayer.GetComponent<PlayerOnCollision> ().ChangeOtherNickname (nickN, gameObject.name);
		}
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
		int deathsIndex = 0;

		for (int i = 0; i < allPlayersRealNames.Count; i++) 
		{
			if (allPlayersRealNames [i] == Rname) {
				deathsIndex = i;
				break;
			}
		}

		allPlayersdeath [deathsIndex].GetComponent<Text>().text = death.ToString ();
	}

	public void ChangeOtherRez(int rezs, string Rname)
	{
		int deathsIndex = 0;

		for (int i = 0; i < allPlayersRealNames.Count; i++) 
		{
			if (allPlayersRealNames [i] == Rname) {
				deathsIndex = i;
				break;
			}
		}

		allPlayersRez [deathsIndex].GetComponent<Text>().text = rezs.ToString ();
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
}


