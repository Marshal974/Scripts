using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	void OnTriggerEnter(Collider otherC) 
	{
		if (otherC.gameObject.name == "Player")
		{
			Debug.Log ("DONE");
		}
}
}
