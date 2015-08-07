using UnityEngine;
using System.Collections;

public class WeaponEffects : MonoBehaviour {

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	public Transform BulletHitPrefab;
	public Transform BloodSplatterPrefab;

	Transform FirePoint;
	Quaternion MuzzleFlashRotation;

	void Awake(){
		FirePoint = transform.GetChild(0);
		if(FirePoint == null){
			Debug.LogError ("No 'FirePoint' object found.");
		}
	}
	
	public void BulletTrail(Vector3 hitPosition){
		// Instantiate the bullet trail and get instantiated trail's LineRenderer 
		Transform trail = Instantiate (BulletTrailPrefab, FirePoint.position, FirePoint.rotation) as Transform;
		LineRenderer linerenderer = trail.GetComponent<LineRenderer> ();

		// Set LineRenderer's start and end position
		if(linerenderer != null){
			linerenderer.SetPosition(0, FirePoint.position);
			linerenderer.SetPosition(1, hitPosition);
		}

		// Destroy the bullet trail
		Destroy (trail.gameObject, 0.04f);
	}

	public void BulletHit(Vector3 hitDirection ,Vector3 hitPosition, Vector3 hitNormal, Collider2D collider){
		if(collider.CompareTag("enemy") || collider.CompareTag("Player") || collider.CompareTag("boss")){
			// Instantiate the bloodsplatter to the opposite direction of the hit
			Transform bloodParticle = Instantiate (BloodSplatterPrefab, hitPosition, Quaternion.FromToRotation (Vector3.back, -hitDirection)) as Transform;

			// Destroy the bloodsplatter effect
			Destroy (bloodParticle.gameObject, 1.0f);
		}
		else if(collider != null){
			// Instantiate the bullet hit effect
			Transform hitParticle = Instantiate (BulletHitPrefab, hitPosition, Quaternion.Inverse(Quaternion.FromToRotation (Vector3.back, hitNormal))) as Transform;

			// Destroy the bullet hit effect
			Destroy (hitParticle.gameObject, 1.0f);
		}
	}

	public void MuzzleFlash() {
		// Set the muzzle flash rotation
		MuzzleFlashRotation = Quaternion.Euler (-FirePoint.rotation.eulerAngles.z, 90, 0f);

		// Instantiate the muzzleflash
		Transform clone = Instantiate (MuzzleFlashPrefab, FirePoint.position, MuzzleFlashRotation) as Transform;

		// Randomize the muzzle flash size a little
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);

		// Destroy the muzzle flash effect
		Destroy (clone.gameObject, 2.0f);
	}
}
