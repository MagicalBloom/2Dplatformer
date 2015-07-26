using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public enum aimTowards {mouse, player};

	public int damage = 10;
	public float aimDelay;
	public LayerMask whatToHit;
	public aimTowards target;

	public Transform aimTestPrefab;
	WeaponEffects weaponEffects;
	GameObject crosshair;

	float playerDistance;
	Player player; //test
	Enemy enemy;

	float hitRange = 100;
	float aimingComplete = 0;
	float shootComplete;
	Transform firePoint;
	Transform arm;


	void Awake () {
		player = GameObject.Find("Player").GetComponent<Player>(); //test
		firePoint = transform.FindChild ("FirePoint");
		arm = GameObject.Find ("Player/Arm").transform;
		weaponEffects = GetComponent<WeaponEffects> ();

		crosshair = GameObject.Find("Crosshair");
		crosshair.GetComponent<SpriteRenderer>().enabled = false;

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
		// Check which aiming mode is selected and do stuff accordingly
		//if (Input.GetKeyDown(KeyCode.Space))
		//	Debug.Log ("asdasdasd");
		if (target == aimTowards.mouse) {
			if (Input.GetMouseButton (0)) {
				// Do some sort of aim effect when holding the mouse button
				if (aimingComplete < aimDelay) {
					aimingComplete += Time.deltaTime;
					//Aim ();
					// Shoot after the mouse button is held down for required time
				} else if (aimingComplete > aimDelay) {
					// Get mouse position from screen and convert that position to the game world + get the position of fire point
					Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
					Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);

					//Randomize aim
					/*
					float minValX = mousePosition.x - 0.2f;
					float maxValX = mousePosition.x + 0.2f;
					float minValY = mousePosition.y - 0.2f;
					float maxValY = mousePosition.y + 0.2f;

					mousePosition.x = Random.Range(minValX,maxValX);
					mousePosition.y = Random.Range(minValY,maxValY);
					*/

					Debug.Log ("SHOOT");
					aimingComplete = 0;
					Shoot (mousePosition, firePointPosition);
				}



			} else if (Input.GetMouseButtonUp (0)) {
				aimingComplete = 0; // Reset the timer for aiming
				crosshair.GetComponent<SpriteRenderer> ().enabled = false; // hide crosshair
			}
		} else if (target == aimTowards.player) {
			//Enemy aiming logic... this might not work but I'll leave it here anyway
			playerDistance = Vector3.Distance (player.transform.position, transform.position);

			// Check if player is close enough to start aiming
			if (playerDistance < 20.0f) { //playerDistance < 20.0f
				if (shootComplete < aimDelay) {
					shootComplete += Time.deltaTime;
				} else if (shootComplete > aimDelay) {
					shootComplete = 0;
					Debug.Log ("ENEMY SHOOT");
					Vector3 playerPosition = new Vector3(player.transform.position.x + player.GetComponent<BoxCollider2D>().offset.x, 
					                                     player.transform.position.y + player.GetComponent<BoxCollider2D>().offset.y,
					                                     0f);
					//Debug.DrawRay (firePoint.position, (playerPosition-firePoint.position)*30, Color.white, 1);
					Shoot (playerPosition, firePoint.position);
				}
			}
		}
	}

	void Aim(){
		/*
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
		*/

		//Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//aimTestPrefab.FindChild ("bullet").GetComponent<SpriteRenderer> ().enabled = true;
		//aimTestPrefab.position = mousePosition;

		if (GameObject.Find ("Crosshair") != null) {
			crosshair.GetComponent<SpriteRenderer>().enabled = true;
			crosshair.transform.position = firePoint.position;    //new Vector3(arm.position.x, arm.position.y + 0.19f, 0f);

			//rotation
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 5.23f;
			
			Vector3 objectPos = Camera.main.WorldToScreenPoint (firePoint.position);
			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;
			
			float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			crosshair.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			/*
			Debug.Log ("AIM");
			Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
			Vector3 direction; 
			direction = mousePosition - firePointPosition;
			direction += firePoint.position;//.normalized;
			
			Quaternion crosshairRotation = Quaternion.Euler (0f, 0f, firePoint.rotation.eulerAngles.z);

			LineRenderer linerenderer = crosshair.GetComponent<LineRenderer> ();
			
			if(linerenderer != null){
				linerenderer.SetPosition(0, firePoint.position);
				linerenderer.SetPosition(1, direction);
			}
			//crosshair.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
			//crosshair.transform.position = new Vector3(firePoint.position.x, firePoint.position.y, 0f);
			//crosshair.transform.rotation = firePoint.rotation;
			*/
		}
		
	}


	void Shoot(Vector2 aimPosition, Vector2 firePointPosition){
		Vector3 hitDirection;
		Vector3 hitPosition; 
		Vector3 hitNormal;
		Collider2D collider;
		
		// raycast is used to see when player hits something
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, aimPosition-firePointPosition, hitRange, whatToHit);

		if (hit.collider != null) {
			// Logic after we hit something
			Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");

			hitDirection = aimPosition-firePointPosition; // direction of the bullet
			hitPosition = hit.point; // point where bullet collided with something
			hitNormal = hit.normal; // normal vector of collided surface
			collider = hit.collider; // for different hit collision effects based on collision

			if(hit.collider.tag == "enemy") {
				hit.collider.GetComponent<Enemy>().DamageEnemy(damage);
			} else if(hit.collider.tag == "hittable") {
				 // wall
			} else if(hit.collider.tag == "Player") {
				player.DamagePlayer(damage);
			} else{
				//do something
			}

			weaponEffects.BulletTrail(hitPosition);
			weaponEffects.BulletHit(hitDirection ,hitPosition, hitNormal, collider);
		} else {
			hitPosition = (aimPosition-firePointPosition)*30; // Get the mouse cursor direction and add some distance
			hitPosition += firePoint.position;
			hitNormal = new Vector3 (9999, 9999, 9999); // used for checking if bullet hit something in effect method... kinda bad solution but whatever
			collider = null;
			weaponEffects.BulletTrail(hitPosition);
		}
		weaponEffects.MuzzleFlash ();


		//Debug.DrawRay (firePointPosition, (mousePosition-firePointPosition)*30, Color.white, 1);

	}
}