﻿using UnityEngine;
using System.Collections;


public class FaceCamera : MonoBehaviour {


	void FixedUpdate () {

			this.transform.LookAt (Camera.main.transform.position);
			this.transform.Rotate (new Vector3 (0, 180, 0));

	}
}
