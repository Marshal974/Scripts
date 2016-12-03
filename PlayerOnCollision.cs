using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerOnCollision : NetworkBehaviour {
//
//	Vector3 spawnSpot;
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
	private int DeathCount = 0;
	public AudioClip onDeathSound;
	public AudioClip onRezSound;
	public string isHeSavior;
	[SyncVar]
	public string saviorName = "pupute";


	[SyncVar]
	public string pNameOnPlayer;
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
//		if (otherC.gameObject.name == "Goal") {
//			currentScene += 1;
//			DontDestroyOnLoad (gameObject);
//			SceneManager.LoadScene (currentScene);
//			transform.position = spawnSpot;
//			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
//				
//		}
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
		AudioSource.PlayClipAtPoint (onDeathSound, gameObject.transform.position);
		if (isLocalPlayer) {
			GameObject.Find ("RespawnBtn").GetComponent<Button> ().enabled = true;
			GameObject.Find ("RespawnBtn").GetComponent<Image> ().enabled = true;
			gameObject.GetComponent<PlayerController> ().enabled = false;
			DeathCount++;
			nbrDeathText.text = DeathCount.ToString ();
			gameObject.GetComponent<PlayerKillCam> ().FindNewTarget ();
		}


		alive = false;
		rb.useGravity = false;
		rb.isKinematic = true;
		if (!isLocalPlayer) {
			SetEmiUp ();
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
		alive = true;
		gameObject.GetComponent<BoxCollider> ().isTrigger = false; 
		rb.useGravity = true;
		Instantiate (lifeEffect, transform.position, Quaternion.identity);
		rb.velocity = Vector3.zero;
		rb.isKinematic = false;
		AudioSource.PlayClipAtPoint (onRezSound, gameObject.transform.position);
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

}


