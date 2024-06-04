using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollJump : MonoBehaviour {

	public float rotationSpeed = 25.0F;
	public float jumpHeight = 8.0F;

	private bool isFalling = false;

	private Rigidbody rigid;

	void Start () {
		rigid = GetComponent<Rigidbody> ();
	}
		
	void Update () {
        /*
		//Handles the rotation of the ball
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		rotation *= Time.deltaTime;
		if (rigid != null) {
			//Apply rotation
			Vector3 vector = new Vector3(rotation,0.0F,0.0F);
			rigid.AddForce (vector, ForceMode.Impulse);
			if (UpKey() && !isFalling) {
				//Jump
				rigid.AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
			}
		}*/
        if (UpKey() && !isFalling) {
				//Jump
				rigid.AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
			}

	}

	public void OnCollisionStay (Collision col) { //Takes parameter of Collision so unity doesn't complain
		isFalling = false;
	}

	public void OnCollisionExit() {
		isFalling = true;
	}

	private bool UpKey() {
		return (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space));
	}

}