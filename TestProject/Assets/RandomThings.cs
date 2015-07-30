using UnityEngine;
using System.Collections;

public class RandomThings : MonoBehaviour {

	/*
		 * For bullet+rigidbody
		Vector3 firePosition = Camera.main.WorldToScreenPoint (firePoint.position);
		Vector3 direction = (Input.mousePosition - firePosition).normalized;

		Quaternion muzzleFlashRotation = Quaternion.Euler (-firePoint.rotation.eulerAngles.z, 90, 0f);

		GameObject bulletClone = Instantiate (BulletPrefab, firePoint.position, muzzleFlashRotation) as GameObject;
		bulletClone.GetComponent<Rigidbody> ().velocity = direction * bulletVelocity;
		Destroy (bulletClone.gameObject, 1f);

		//bullet effect reflection
		v' = 2 * (v . n) * n - v;
		v = hitPosition
		n = hit.normal
		*/
	//Quaternion asd = 2 * (Vector3.Dot(hitPosition,hitNormal)) * hitNormal - hitPosition as Quaternion;





	/*SHOOT
			if(ShootComplete > aimDelay) {
				if (Input.GetMouseButton (0)) {
					Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
					Vector2 firePointPosition = new Vector2 (FirePoint.position.x, FirePoint.position.y);
					//Debug.Log(FirePoint.parent.parent.TransformVector(FirePoint.forward)); //.InverseTransformDirection(transform.forward)

					Shoot (mousePosition, firePointPosition);
					ShootComplete = 0;
				} 
			} else if (ShootComplete < aimDelay) {
				ShootComplete += Time.deltaTime;

			}
			*/
	/*
			if (Input.GetMouseButton (0)) {
				// Do some sort of aim effect when holding the mouse button
				if (AimingComplete < aimDelay) {
					AimingComplete += Time.deltaTime;
					//Aim ();
					// Shoot after the mouse button is held down for required time
				} else if (AimingComplete > aimDelay) {
					// Get mouse position from screen and convert that position to the game world + get the position of fire point
					Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
					Vector2 firePointPosition = new Vector2 (FirePoint.position.x, FirePoint.position.y);

					//Randomize aim

					float minValX = mousePosition.x - 0.2f;
					float maxValX = mousePosition.x + 0.2f;
					float minValY = mousePosition.y - 0.2f;
					float maxValY = mousePosition.y + 0.2f;

					mousePosition.x = Random.Range(minValX,maxValX);
					mousePosition.y = Random.Range(minValY,maxValY);


					Debug.Log ("SHOOT");
					AimingComplete = 0;
					Shoot (mousePosition, firePointPosition);
				}



			} else if (Input.GetMouseButtonUp (0)) {
				AimingComplete = 0; // Reset the timer for aiming
				Crosshair.GetComponent<SpriteRenderer> ().enabled = false; // hide crosshair
			}
			*/






	/*AIM
		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (FirePoint.position.x, FirePoint.position.y);
		Vector3 mouseDirection = mousePosition - firePointPosition;
		mouseDirection.z = 0f;

		// draw some kind of effect for aiming...WIP
		Transform line = Instantiate (AimTestPrefab, FirePoint.position, FirePoint.rotation) as Transform;
		LineRenderer linerenderer = line.GetComponent<LineRenderer> ();

		if(linerenderer != null){
			linerenderer.SetPosition(0, FirePoint.position);
			linerenderer.SetPosition(1, mousePosition);
		}
		Destroy (line.gameObject, 0.04f);
		*/
	
	//Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	//AimTestPrefab.FindChild ("bullet").GetComponent<SpriteRenderer> ().enabled = true;
	//AimTestPrefab.position = mousePosition;

	/*
			Debug.Log ("AIM");
			Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			Vector2 firePointPosition = new Vector2 (FirePoint.position.x, FirePoint.position.y);
			Vector3 direction; 
			direction = mousePosition - firePointPosition;
			direction += FirePoint.position;//.normalized;
			
			Quaternion crosshairRotation = Quaternion.Euler (0f, 0f, FirePoint.rotation.eulerAngles.z);

			LineRenderer linerenderer = crosshair.GetComponent<LineRenderer> ();
			
			if(linerenderer != null){
				linerenderer.SetPosition(0, FirePoint.position);
				linerenderer.SetPosition(1, direction);
			}
			//crosshair.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
			//crosshair.transform.position = new Vector3(FirePoint.position.x, FirePoint.position.y, 0f);
			//crosshair.transform.rotation = FirePoint.rotation;
			*/

	/*
		if (GameObject.Find ("Crosshair") != null) {
			Crosshair.GetComponent<SpriteRenderer>().enabled = true;
			Crosshair.transform.position = FirePoint.position;

			//rotation
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 5.23f;
			
			Vector3 objectPos = Camera.main.WorldToScreenPoint (FirePoint.position);
			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;
			
			float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			Crosshair.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		}
		*/
}
