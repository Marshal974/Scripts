using UnityEngine;
using System.Collections;

public class ShowTab : MonoBehaviour {
//	private bool actifTab;
	private GameObject ChildTab;

	void Awake(){
		ChildTab = gameObject;
		ChildTab.GetComponent<Canvas>().enabled = false;	}
	void Update(){

		if (Input.GetKeyDown (KeyCode.Space)){
			
			ChildTab.GetComponent<Canvas>().enabled = true;   //			actifTab = true;			
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			ChildTab.GetComponent<Canvas>().enabled = false;
		}
	}	
}
