using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public enum AttachedTo {player, enemy};

	public bool FreezeArmAndDirection = false;

	public int RotationOffset = 0; // In case sprite is drawn in wrong angle
	private Transform FirePoint;

	public AttachedTo attachedTo; // Is the arm attached to player or enemy?

	private bool MouseRightSide = true; // For determining which way the player is currently aiming.
	private Vector3 Direction;
	private Player player;
	private float RotationZ;


	private void Awake() {
		player = GameObject.Find("Player").GetComponent<Player>();
		FirePoint = this.transform.GetChild (0).GetChild (0);
	}

	void Update () {
		// If player dies
		if(player == null)
			return;

		Vector3 armScale = this.transform.localScale;
		Vector3 lastDirection = Direction;
		float lastRotation = RotationZ;

		// Calculate player arm rotation based on mouse position
		if (attachedTo == AttachedTo.player) {
			Direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

			Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition)- transform.position;		// Subtract the position of the player from the mouse position
			difference.Normalize ();	// Normalize the vector
			
			RotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees
		
		// Calculate enemy arm rotation based on player position
		} else {
			Vector3 difference = player.transform.position - transform.position;	// Subtract the position of the player from the enemy position
			difference.Normalize ();	// Normalize the vector

			// For enemy aim effects
			if(FreezeArmAndDirection == true){
				Direction = lastDirection;
				RotationZ = lastRotation;
			} else {
				Direction = player.transform.position - FirePoint.position;
				RotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees
			}
		}

		// If the target is on the right side of the arm's owner
		if (Direction.x >= 0) {

			// Flip the scale of the sprite
			if(MouseRightSide == false) {
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			MouseRightSide = true; // Set boolean to the opposite so scale flip will happen only once

			// Assign rotation to the arm and firepoint(so bullets fly in the right direction)
			transform.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
			FirePoint.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
		}
		// If the target is on the left side of the arm's owner
		else {

			// Flip the scale of the sprite
			if(MouseRightSide == true){
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			MouseRightSide = false; // Set boolean to the opposite so scale flip will happen only once

			// Assign rotation to the arm and firepoint(so bullets fly in the right direction)
			transform.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset + 180); 
			FirePoint.rotation = Quaternion.Euler (0f, 0f, RotationZ + RotationOffset);
		}
	}
}
