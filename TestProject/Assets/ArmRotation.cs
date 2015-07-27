using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public enum AttachedTo {player, enemy};

	public int rotationOffset = 0;
	private Transform firePoint;
	public AttachedTo attachedTo;

	private bool mouseRightSide = true; // For determining which way the player is currently aiming.
	private Vector3 direction;
	private Player player;
	private float rotationZ;


	private void Awake() {
		player = GameObject.Find("Player").GetComponent<Player>();
		firePoint = this.transform.GetChild (0).GetChild (0).transform;
	}

	// Update is called once per frame
	void Update () {
		Vector3 armScale = this.transform.localScale;

		if (attachedTo == AttachedTo.player) {
			direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

			Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition)- transform.position;		// Subtract the position of the player from the mouse position
			difference.Normalize ();	// Normalize the vector
			
			rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees

		} else {
			direction = player.transform.position - firePoint.position;

			Vector3 difference = player.transform.position - transform.position;	// Subtract the position of the player from the mouse position
			difference.Normalize ();	// Normalize the vector
			
			rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees
		}

		if (direction.x >= 0) {
			if(mouseRightSide == false) {
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			mouseRightSide = true;
			transform.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
			firePoint.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
		}
		else {
			if(mouseRightSide == true){
				armScale.x *= -1;
				this.transform.localScale = armScale;
			}

			mouseRightSide = false;
			transform.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset + 180);
			firePoint.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
		}
	}
}
