﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAnimNonPlayer : NetworkBehaviour {

	public float moveSpeed;
//	float maxSpeed = 10f;
	Vector3 inputV;
	public float rotationSpeed = 100f;
	Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = this.GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void Update ()
	{
		if (isLocalPlayer) 
		{
			return;
		}
//
//		if (rb.velocity.magnitude > (maxSpeed * 90) / 100) {
//			StartCoroutine(PlayAnim("Run",0.1f));
//		} else if (rb.velocity.magnitude < (maxSpeed * 90) / 100 && rb.velocity.magnitude > (maxSpeed * 2) / 100) {
//			StartCoroutine (PlayAnim ("IdleSit", 2f));
//		} else {
			StartCoroutine (PlayAnim ("Idle", 5f));
//		}

	}

	IEnumerator PlayAnim(string animName, float animTime)
	{
		gameObject.GetComponentInChildren<Animation> ().Play (animName);
		yield return new WaitForSeconds (animTime);
	}
}



