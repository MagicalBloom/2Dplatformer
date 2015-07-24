using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public enum aimTowards {mouse, player};

	public float damage = 10;
	public float aimDelay;
	public LayerMask whatToHit;
	public aimTowards target;

	public Transform aimTestPrefab;
	WeaponEffects weaponEffects;

	Player player; //test

	float hitRange = 100;
	float aimingComplete = 0;
	Transform firePoint;
	Transform arm;


	void Awake () {
		player = GameObject.Find("Player").GetComponent<Player>(); //test
		firePoint = transform.FindChild ("FirePoint");
		arm = GameObject.Find ("Player/Arm").transform;
		weaponEffects = GetComponent<WeaponEffects> ();

		if(firePoint == null){
			Debug.LogError ("No 'firePoint' object found.");
		}
		if(arm == null){
			Debug.LogError ("No 'arm' object found.");
		}
		if(weaponEffects == null) {
			Debug.LogError ("Script for weapon effects is missing!");
		}
	}

	void Update () {
		if(target == aimTowards.mouse){
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
		} else if(target == aimTowards.player){
			//Enemy aiming logic... this might not work but I'll leave it here anyway
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
		Vector3 hitPosition; 
		Vector3 hitNormal;
		Collider2D collider;

		// Get mouse position from screen and convert that position to the game world
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		// raycast is used to see when player hits something
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, hitRange, whatToHit);

		if (hit.collider != null) {
			// Logic after we hit something
			Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");

			hitPosition = hit.point; // point where bullet collided with something
			hitNormal = hit.normal; // normal vector of collided surface
			collider = hit.collider; // for different hit collision effects based on collision

			if(hit.collider.tag == "enemy") {
				//do something
			} else if(hit.collider.tag == "hittable") {
				player.DamagePlayer(50); //test
			} else{
				//do something
			}

			weaponEffects.BulletTrail(hitPosition);
			weaponEffects.BulletHit(hitPosition, hitNormal, collider);
		} else {
			hitPosition = (mousePosition-firePointPosition)*30; // Get the mouse cursor direction and add some distance
			hitPosition += firePoint.position;
			hitNormal = new Vector3 (9999, 9999, 9999); // used for checking if bullet hit something in effect method... kinda bad solution but whatever
			collider = null;
			weaponEffects.BulletTrail(hitPosition);
		}
		weaponEffects.MuzzleFlash ();


		//Debug.DrawRay (firePointPosition, (mousePosition-firePointPosition)*30, Color.white, 1);

	}
}