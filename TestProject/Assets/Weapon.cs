using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float damage = 10;
	public float range = 100;
	public float bulletVelocity = 100;
	public float aimDelay;
	public LayerMask whatToHit;

	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	public Transform BulletHitPrefab;
	public Transform aimTestPrefab;

	//Need these to test healthbar
	GameObject player;
	PlayerHealth playerHealth;

	float aimingComplete = 0;
	Transform firePoint;
	Transform arm;
	Quaternion muzzleFlashRotation;


	// Use this for initialization
	void Awake () {
		//Need these to test healthbar
		player = GameObject.Find ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();

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
			// Do some sort of aim effect when holding the mouse button
			if(aimingComplete < aimDelay){
				aimingComplete += Time.deltaTime;
				//Aim ();
			// Shoot after the mouse button is held down for required time
			} else if(aimingComplete > aimDelay){
				Debug.Log("SHOOT");
				aimingComplete = 0;
				Shoot ();
			}
		}
		else if(Input.GetMouseButtonUp(0)) {
			aimingComplete = 0; // Reset the timer for aiming
		}
	}


	void Aim(){
		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		Vector3 mouseDirection = mousePosition - firePointPosition;
		mouseDirection.z = 0f;

		// draw some kind of effect for aiming...WIP
		Transform line = Instantiate (aimTestPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer linerenderer = line.GetComponent<LineRenderer> ();

		if(linerenderer != null){
			linerenderer.SetPosition(0, firePoint.position);
			linerenderer.SetPosition(1, mousePosition);
		}
		Destroy (line.gameObject, 0.04f);
	}


	void Shoot(){
		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		// raycast is used to see when player hits something
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, range, whatToHit);

		if(hit.collider != null){
			// Logic after we hit something
			Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");
			playerHealth.playerTakeDamage(20);
		}
		
		Vector3 hitPosition; 
		Vector3 hitNormal;

		if(hit.collider == null){
			hitPosition = (mousePosition-firePointPosition)*30; // Get the mouse cursor direction and add some distance
			hitPosition += firePoint.position;
			hitNormal = new Vector3 (9999, 9999, 9999); // used for checking if bullet hit something in effect method... kinda bad solution but whatever
		} else {
			hitPosition = hit.point; // point where bullet collided with something
			hitNormal = hit.normal; // normal vector of collided surface
		}
		//Debug.DrawRay (firePointPosition, (mousePosition-firePointPosition)*30, Color.white, 1);
		Effect (hitPosition, hitNormal);


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


	void Effect(Vector3 hitPosition, Vector3 hitNormal){
		// Bullet trail effect
		// Basically we move pre-made bullet trail from one point to another
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer linerenderer = trail.GetComponent<LineRenderer> ();

		if(linerenderer != null){
			linerenderer.SetPosition(0, firePoint.position);
			linerenderer.SetPosition(1, hitPosition);
		}
		Destroy (trail.gameObject, 0.04f);

		// Bullet hit effect
		if(hitNormal != new Vector3(9999, 9999, 9999)){
			Transform hitParticle = Instantiate (BulletHitPrefab, hitPosition, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
			Destroy (hitParticle.gameObject, 1.0f);
		}

		//Muzzle flash effect
		muzzleFlashRotation = Quaternion.Euler (-firePoint.rotation.eulerAngles.z, 90, 0f);
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, muzzleFlashRotation) as Transform;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, 0);
		Destroy (clone.gameObject, 2.0f);
	}
}