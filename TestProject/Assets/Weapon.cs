using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float Damage = 10;
	public float Range = 100;
	public LayerMask whatToHit;
	public GameObject crosshair;

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	public Transform BulletHitPrefab;
	//float timeToSpawnEffect = 0;
	//public float effectSpawnRate = 10;

	float aimingComplete = 0;
	float timeToFire = 0;
	Transform firePoint;
	Transform arm;
	Quaternion muzzleFlashRotation;


	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		arm = GameObject.Find ("Player/Arm").transform;
		if(firePoint == null){
			Debug.LogError ("No 'firePoint' object found.");
		}
		if(arm == null){
			Debug.LogError ("No 'arm' object found.");
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){
			if(aimingComplete < 1.5){
				aimingComplete += Time.deltaTime;
				Aim ();
			} else if(aimingComplete > 1.5){
				Debug.Log("SHOOT");
				aimingComplete = 0;
				Shoot ();
			}
		}
		else if(Input.GetMouseButtonUp(0)) {
			aimingComplete = 0;
		}
	}


	void Aim(){
		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		Vector3 mouseDirection = mousePosition - firePointPosition;

		// draw some kind of effect for aiming...WIP
		Transform line = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer linerenderer = line.GetComponent<LineRenderer> ();

		if(linerenderer != null){
			linerenderer.SetPosition(0, firePoint.position);
			linerenderer.SetPosition(1, mouseDirection);
		}
		Destroy (line.gameObject, 0.04f);
	}


	void Shoot(){
		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, Range, whatToHit);

		if(hit.collider != null){
			Debug.Log ("We hit " + hit.collider.name + " and did " + Damage + " damage.");
		}
		
		Vector3 hitPosition; // point where bullet collided with something
		Vector3 hitNormal;

		if(hit.collider == null){
			hitPosition = (mousePosition - firePointPosition) * 30; // Get the mouse cursor direction and add some distance
			hitNormal = new Vector3 (9999, 9999, 9999);
		} else {
			hitPosition = hit.point;
			hitNormal = hit.normal;
		}

		Effect (hitPosition, hitNormal);
	}


	void Effect(Vector3 hitPosition, Vector3 hitNormal){
		//Bullet trail effect
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer linerenderer = trail.GetComponent<LineRenderer> ();

		if(linerenderer != null){
			linerenderer.SetPosition(0, firePoint.position);
			linerenderer.SetPosition(1, hitPosition);
		}
		Destroy (trail.gameObject, 0.04f);

		//Bullet hit effect
		if(hitNormal != new Vector3(9999, 9999, 9999)){
			Transform hitParticle = Instantiate (BulletHitPrefab, hitPosition, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
			Destroy (hitParticle.gameObject, 1.0f);
		}

		//Muzzle flash effect
		muzzleFlashRotation = Quaternion.Euler (-firePoint.rotation.eulerAngles.z, 90, 0f);
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, muzzleFlashRotation) as Transform;
		//clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 2.0f);
	}
}