using UnityEngine;
using System.Collections;

public class ShowTab : MonoBehaviour {
//	private bool actifTab;
	private GameObject ChildTab;

	void Awake(){
		ChildTab = gameObject;
		ChildTab.GetComponent<Canvas>().enabled = false;	}
	void Update(){

		if (Input.GetKeyDown (KeyCode.Tab)){
			
			ChildTab.GetComponent<Canvas>().enabled = true;   //			actifTab = true;			
		}
		if (Input.GetKeyUp (KeyCode.Tab)) {
			ChildTab.GetComponent<Canvas>().enabled = false;
		}
	}	
}
