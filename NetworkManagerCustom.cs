using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager 
{
	public AudioClip bip1;
	public AudioClip bip2;

	public void StartUpHost()
	{
		AudioSource.PlayClipAtPoint(bip1, gameObject.transform.position);
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame ()
	{
		AudioSource.PlayClipAtPoint(bip1, gameObject.transform.position);
		SetIPAdress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();

	}
	public void ConnectToLobby ()
	{
		AudioSource.PlayClipAtPoint(bip2, gameObject.transform.position);

	}
	public void QuitGame ()
	{
		Application.Quit ();

	}

	void SetIPAdress()
	{
		string ipAdress = GameObject.Find ("InputFieldIPAdress").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = ipAdress;
	}
	void SetPort()
	{

		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded (int level)
	{
		if (level == 0) {
			StartCoroutine(SetupMenuSceneButtons());
								
		} else 
		{
			StartCoroutine(SetupOtherSceneButtons ());
		}
			
	}

	IEnumerator SetupMenuSceneButtons()
	{
		yield return new WaitForSeconds(0.3f);
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.AddListener (StartUpHost);

		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.AddListener (JoinGame);

	}
	IEnumerator SetupOtherSceneButtons()
	{
		
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("ButtonDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("ButtonDisconnect").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopHost);
	}

}
