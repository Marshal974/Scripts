using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerOnCollision : NetworkBehaviour {

	Vector3 spawnSpot;
//	private string saviorNameID;
	public GameObject deathEffect;
	public int currentScene = 0;
	public Rigidbody rb;
	public GameObject repere;
	public Material matMort;
	public Material normalMat;
	[SyncVar]bool alive = true;
	public GameObject lifeEffect;
	private GameObject nbrDeathObj ;
	private Text nbrDeathText;
	private GameObject nbrRezObj ;
	private Text nbrRezText;
	private GameObject messObj ;
	private Text PlayerAnnounce;
	private int DeathCount = 0;
	private int RezCount = 0;
	[SyncVar]
	public string saviorName = "pupute";

	[SyncVar]
	public string pNameOnPlayer;

	// Use this for initialization
	void Start () {

		pNameOnPlayer = this.GetComponent<ChoosePlayerName> ().pname; // !!!!a faire plus tard (genre en update) ou dés que ca change...le prob vient de la.
		spawnSpot = transform.position;
		rb = this.GetComponent<Rigidbody> ();		
		messObj = GameObject.Find("LocalMessage");
		nbrDeathObj = GameObject.Find ("NbrDeath");
		nbrDeathText = nbrDeathObj.GetComponent<Text> ();
		PlayerAnnounce = messObj.GetComponent<Text> ();
		nbrRezObj = GameObject.Find ("NbrRez");
		nbrRezText = nbrRezObj.GetComponent<Text> ();
	}
		
	void OnCollisionEnter (Collision other) {
//		if (iscl) {
		//check if its an ennemy
//		if (other.gameObject.tag == "BadGuys" && alive == true && isServer) {
//				RpcOnDeath (); //contact on server.
//
//			}
		if (other.gameObject.tag == "BadGuys" && alive == true && isLocalPlayer) {
			CmdIAmDead ();
//Contact on client : if it is, tell all clients this player is dead.

		}
//		}
	}
	[Command]
	public void CmdIAmDead(){
		RpcOnDeath ();
	}
		void OnTriggerEnter(Collider otherC)
	{
		if (otherC.gameObject.name == "Goal") {
			currentScene += 1;
			DontDestroyOnLoad (gameObject);
			SceneManager.LoadScene (currentScene);
			transform.position = spawnSpot;
			gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
				
		}
		if (isLocalPlayer) {
			if (otherC.gameObject.tag == "Player" && alive == false) {
			saviorName = otherC.GetComponent<PlayerOnCollision>().pNameOnPlayer;
			CmdTellThemAll (saviorName, pNameOnPlayer);
//			otherC.gameObject.name = saviorNameID;
//			Debug.Log (saviorNameID);

			}
		}
	}
		
	[ClientRpc]
	public void RpcOnDeath() 
	{
		Instantiate (deathEffect, transform.position, Quaternion.identity);
		gameObject.GetComponent<BoxCollider>().isTrigger = true; 
		rb.velocity = Vector3.zero;
		if (isLocalPlayer) {
			gameObject.GetComponent<PlayerController> ().enabled = false;
		}
//		gameObject.GetComponent<BoxCollider> ().enabled = false;
//		Instantiate (repere, transform.position, Quaternion.identity);
		alive = false;
		rb.useGravity = false;
		rb.isKinematic = true;
		if (isLocalPlayer) {
			DeathCount++;
			nbrDeathText.text = DeathCount.ToString ();
		}
		//transform.position = spawnSpot;
	}
	[ClientRpc]
	public void RpcOnRez()
	{
		if (isLocalPlayer) {
			gameObject.GetComponent<PlayerController> ().enabled = true;
		}
			alive = true;
			gameObject.GetComponent<BoxCollider>().isTrigger = false; 
			rb.useGravity = true;
			Instantiate (lifeEffect, transform.position, Quaternion.identity);
		    rb.velocity = Vector3.zero;
		rb.isKinematic = false;
//		}
	}

	string ClientLocalMessage(string otherCName){
		string totalMess;
//
//		if (isLocalPlayer) {
			
			//			messObj.GetComponent<Text> ().text = messTxTFinal;
			totalMess = otherCName + " just saved your ass! Run kitty Run!";
//			CmdTellThemAll (saviorName, pNameOnPlayer);
			return totalMess;
//
//		} else {
//			return "";
//		}
	}

	[Command]
	void CmdTellThemAll(string savior, string saved){
//		saviorName = savior;
		RpcShowRespects (savior, saved);
		RpcOnRez ();
	}
	[ClientRpc]
	void RpcShowRespects(string savior, string saved){
		if (isLocalPlayer) {
			Debug.Log (savior);
			PlayerAnnounce.text = ClientLocalMessage (savior);
			
		} else if (savior.Equals (this.pNameOnPlayer)) {
			PlayerAnnounce.text = "You just saved " + saved + ". That's my kitty !";
			RezCount++;
			nbrRezText.text = RezCount.ToString ();

		}else
			PlayerAnnounce.text = savior + " just saved " + saved + ". Well Done !";
		Debug.Log (savior);
	}


}
