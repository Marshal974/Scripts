using UnityEngine;
using System.Collections;

public class ShowTab : MonoBehaviour {
//	private bool actifTab;
	private GameObject ChildTab;
	bool forceOpen = false;

	void Awake()
	{
		ChildTab = gameObject;
		ChildTab.GetComponent<Canvas>().enabled = false;	
	}
	void Update(){
		if (forceOpen == true) 
		{
			return;
		}

		if (Input.GetKeyDown (KeyCode.Tab)){
			
			ChildTab.GetComponent<Canvas>().enabled = true;   //			actifTab = true;			
		}
		if (Input.GetKeyUp (KeyCode.Tab)) {
			ChildTab.GetComponent<Canvas>().enabled = false;
		}
	}	
	public void IfGameOverShow()
	{
		ChildTab.GetComponent<Canvas> ().enabled = true;
		forceOpen = true;

	}
}
