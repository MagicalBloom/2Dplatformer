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
}
