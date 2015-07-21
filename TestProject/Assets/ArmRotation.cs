using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 0;

	private bool mouseRightSide = true; // For determining which way the player is currently aiming.

	Transform playerArm;
	Transform playerWeaponFirePoint;

	private void Awake() {
		playerArm = GameObject.Find ("Arm").transform;
		playerWeaponFirePoint = GameObject.Find ("Player/Arm/Pistol/FirePoint").transform;
		if(playerArm == null){
			Debug.LogError ("No 'playerArm' object found.");
		}
		else if(playerWeaponFirePoint == null){
			Debug.LogError ("No 'playerWeaponFirePoint' object found.");
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		Vector3 armScale = playerArm.localScale;

		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition)- transform.position;		// Subtract the position of the player from the mouse position
		difference.Normalize ();	// Normalize the vector
		
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;	// Find the angle in degrees

		if (direction.x >= 0) {
			if(mouseRightSide == false) {
				armScale.x *= -1;
				playerArm.localScale = armScale;
			}

			mouseRightSide = true;
			transform.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
			playerWeaponFirePoint.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
		}
		else {
			if(mouseRightSide == true){
				armScale.x *= -1;
				playerArm.localScale = armScale;
			}

			mouseRightSide = false;
			transform.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset + 180);
			playerWeaponFirePoint.rotation = Quaternion.Euler (0f, 0f, rotationZ + rotationOffset);
		}
	}
}
