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
		Transform trail = Instantiate (BulletTrailPrefab, FirePoint.position, FirePoint.rotation) as Transform;
		LineRenderer linerenderer = trail.GetComponent<LineRenderer> ();
		
		if(linerenderer != null){
			linerenderer.SetPosition(0, FirePoint.position);
			linerenderer.SetPosition(1, hitPosition);
		}
		Destroy (trail.gameObject, 0.04f);
	}

	public void BulletHit(Vector3 hitDirection ,Vector3 hitPosition, Vector3 hitNormal, Collider2D collider){
		if(collider.CompareTag("enemy") || collider.CompareTag("Player")){
			// Instantiate the bloodsplater to the opposite direction of the hit
			Transform bloodParticle = Instantiate (BloodSplatterPrefab, hitPosition, Quaternion.FromToRotation (Vector3.back, -hitDirection)) as Transform;
			Destroy (bloodParticle.gameObject, 1.0f);
		}
		else if(collider != null){
			// Instantiate the bullet hit effect
			Transform hitParticle = Instantiate (BulletHitPrefab, hitPosition, Quaternion.Inverse(Quaternion.FromToRotation (Vector3.back, hitNormal))) as Transform;
			//Transform hitParticle = Instantiate (BulletHitPrefab, hitPosition, Quaternion.Euler(Vector3.Reflect(hitDirection, hitNormal).x*-1, 90f, 0f)) as Transform;
			Destroy (hitParticle.gameObject, 1.0f);
		}
	}

	public void MuzzleFlash() {
		MuzzleFlashRotation = Quaternion.Euler (-FirePoint.rotation.eulerAngles.z, 90, 0f);
		Transform clone = Instantiate (MuzzleFlashPrefab, FirePoint.position, MuzzleFlashRotation) as Transform;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 2.0f);
	}
}
