using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings (channel = 0,sendInterval = 0.033f)]
public class MovementSyncOP : NetworkBehaviour {

	[SyncVar(hook = "SyncPositionValues")]	private Vector3 syncPos;
	[SyncVar(hook = "SyncRotationValues")] private float syncRot;
	[SerializeField] Transform myTransform;
	float lerpRate;
	float normalLerpRate = 12;
	float fasterLerpRate = 30;
	private Vector3 lastPos;
	private float lastRot;
	private float threshold = 0.5f;
	private float rotThreshold = 2f;
	[SerializeField] bool useHistoricalLerping = false;
	private List<Vector3> syncPosList = new List<Vector3>();
	private float closeEnough = 0.2f;
	private float closeEnoughRot =1f;
	private List<float> syncPlayerRotList = new List<float>();



	void Start()
	{
		lerpRate = normalLerpRate;
	}

	void Update()
	{
		LerpPosition ();

	}
	void FixedUpdate () 
	{
		TransmitPosition ();
		TransmitRotation ();

	}
	void LerpPosition()
	{
		if (!isLocalPlayer) 
		{
			if (useHistoricalLerping == true) 
			{
				HistoricalLerping ();
				HistoricalLerpRot ();
			}
			else 
			{
				OrdinaryLerping ();
			}

		}
	}

	void LerpRotation(float rotAngle){
	if (!isLocalPlayer)
	{
		Vector3 playerNewRot = new Vector3 (0, rotAngle, 0);
		myTransform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.Euler (playerNewRot), lerpRate *3.5f* Time.deltaTime);
	}
	}

	[Command]
	void CmdProvidePositionToServer(Vector3 pos)
	{
		syncPos = pos;
	}

	[Command]
	void CmdProvideRotationToServer(float playerRot)
	{
		syncRot = playerRot;
	}

	[ClientCallback]
	void TransmitPosition()
	{
		if (isLocalPlayer) {
			if (Vector3.Distance (myTransform.position, lastPos)> threshold) {
				CmdProvidePositionToServer (myTransform.position);
				lastPos = myTransform.position;
			}
		}
	}
	[ClientCallback]
	void TransmitRotation()
	{
		if (isLocalPlayer) 
		{
			if (CheckIfBeyondThreshold(myTransform.localEulerAngles.y, lastRot))
				{
				lastRot = myTransform.localEulerAngles.y;
				CmdProvideRotationToServer (lastRot);

				}
		}
	}

	void OrdinaryLerping()
	{
		myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);

	}
					

	void HistoricalLerping()
	{
		if (syncPosList.Count > 0) 
		{
			myTransform.position = Vector3.Lerp (myTransform.position, syncPosList [0], Time.deltaTime * lerpRate);

			if (Vector3.Distance (myTransform.position, syncPosList [0]) < closeEnough) {
				syncPosList.RemoveAt (0);
			}
			if (syncPosList.Count > 10) {
				lerpRate = fasterLerpRate;
			} else {
				lerpRate = normalLerpRate;
			}
		}
	}

	void HistoricalLerpRot()
	{
		if (syncPlayerRotList.Count > 0) 
		{
			LerpRotation (syncPlayerRotList [0]);
		
			if (Mathf.Abs (myTransform.localEulerAngles.y - syncPlayerRotList [0]) < closeEnoughRot) 
			{
				syncPlayerRotList.RemoveAt (0);
			}
//			if (syncPlayerRotList.Count > 10) 
//			{
//				lerpRate = fasterLerpRate;
//			} else 
//			{
//				lerpRate = normalLerpRate;
//			}

		}
	}
	[Client]
	void SyncPositionValues(Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}
	[Client]
	void SyncRotationValues(float latestPlayerRotation)
	{
		syncRot = latestPlayerRotation;
		syncPlayerRotList.Add (syncRot);
	}

	bool CheckIfBeyondThreshold(float rot1, float rot2)
	{
		if (Mathf.Abs (rot1 - rot2) > rotThreshold) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}
}
