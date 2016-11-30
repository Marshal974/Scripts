using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	[SyncVar] public float moveSpeed;
	float maxSpeed = 10f;
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
		if (rb.velocity.magnitude < maxSpeed) {
			rb.AddRelativeForce (inputV * moveSpeed * Time.deltaTime);
		}
		inputV = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
		float rotation = Input.GetAxis ("Horizontal") * rotationSpeed;
		rotation *= Time.deltaTime;
		transform.Rotate (0, rotation, 0);
	}
}
		


