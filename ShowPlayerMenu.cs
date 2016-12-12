using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ShowPlayerMenu: MonoBehaviour {
	private bool actifTab;
	private GameObject ChildMenu;
	public Slider myVol; 

	void Awake()
	{
		ChildMenu = gameObject;
		ChildMenu.GetComponent<Canvas>().enabled = false;	
	}
	public void ToggleMenu(){

		if (actifTab == false){

			ChildMenu.GetComponent<Canvas>().enabled = true;
			actifTab = true;
			return;
		}
		if (actifTab == true){

			ChildMenu.GetComponent<Canvas>().enabled = false;
			actifTab = false;			
		}

	}	

	public void ChangeGeneralVolume()
	{
		AudioListener.volume = myVol.value;
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) 
		{
			ToggleMenu ();
		}
	}

}