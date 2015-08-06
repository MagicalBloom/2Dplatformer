using UnityEngine;
using System.Collections;

public class testMovement : MonoBehaviour {

	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;

	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		// Right
		if(Input.GetKey(KeyCode.D)){
			Vector3 temp = new Vector3(0.2f, 0f, 0f);
			this.transform.position += temp;
		}
		// Left
		if(Input.GetKey(KeyCode.A)){
			Vector3 temp = new Vector3(-0.2f, 0f, 0f);
			this.transform.position += temp;
		}
		// Up
		if(Input.GetKey(KeyCode.W)){
			Vector3 temp = new Vector3(0f, 0.2f, 0f);
			this.transform.position += temp;
		}
		// Down
		if(Input.GetKey(KeyCode.S)){
			Vector3 temp = new Vector3(0f, -0.2f, 0f);
			this.transform.position += temp;
		}
	}
}
