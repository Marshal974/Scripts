using UnityEngine;
using System.Collections;

public class ShowTab : MonoBehaviour {
//	private bool actifTab;
	private GameObject ChildTab;

	void Awake(){
//		actifTab = false;
		ChildTab = transform.FindChild("Tabulation").gameObject;
		ChildTab.SetActive (false);
	}
	void Update(){

		if (Input.GetKeyDown (KeyCode.Space)){
			
		ChildTab.SetActive (true);
//			actifTab = true;			
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			ChildTab.SetActive (false);
		}
	}	
}
