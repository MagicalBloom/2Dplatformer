using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public enum AttachedTo {player, enemy};

	public int RotationOffset = 0;
	private Transform FirePoint;
	public AttachedTo attachedTo;

	private bool MouseRightSide = true; // For determining which way the player is currently aiming.
	private Vector3 Direction;
	private Player player;
	private float RotationZ;


	private void Awake() {
		player = GameObject.Find("Player").GetComponent<Player>();
		FirePoint = this.transform.GetChild (0).GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		Vector3 armScale = this.transform.localScale;

		if (attachedTo == AttachedTo.player) {
			Direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

			Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition)- transform.position;		// Subtract the position of the player from the mouse position
			difference.Normalize ();	// Normalize the vector
			
			RotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees

		} else {
			Direction = player.transform.position - FirePoint.position;

			Vector3 difference = player.transform.position - transform.position;	// Subtract the position of the player from the mouse position
			difference.Normalize ();	// Normalize the vector
			
			RotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees
		}

		if (Direction.x >= 0) {
			if(MouseRightSide == false) {
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			MouseRightSide = true;
			transform.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
			FirePoint.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
		}
		else {
			if(MouseRightSide == true){
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			MouseRightSide = false;
			transform.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset + 180);
			FirePoint.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
		}
	}
}
