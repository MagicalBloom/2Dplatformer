using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	[System.Serializable]
	public class WeaponStats{
		public int Damage;
		public float FireRate;
		public WeaponTypes WeaponType;
		public float ReloadTime;
		public int ClipSize;
	}

	public WeaponStats weaponStats = new WeaponStats();

	public enum aimTowards {mouse, player};
	public enum WeaponTypes {full, semi, single};

	public float EnemyAimDelay;
	public LayerMask WhatToHit;
	public aimTowards Target;

	public Transform AimTestPrefab;

	private WeaponEffects weaponEffects;
	private GameObject Crosshair;

	private Player player; //test
	private Enemy enemy;

	private int CurrentAmmo;
	private float RandomAimDelay;
	private float HitRange = 45;
	private float ShootTimer;
	private float ReloadTimer;
	private bool ReloadComplete = true;
	private Transform FirePoint;

	private AudioSource audioSource;
	public AudioClip WeaponFireSoundEffect;


	void Awake () {
		player = GameObject.Find("Player").GetComponent<Player>(); //test
		FirePoint = transform.GetChild(0);
		weaponEffects = GetComponent<WeaponEffects> ();
		RandomAimDelay = EnemyAimDelay;
		CurrentAmmo = weaponStats.ClipSize;

		Crosshair = GameObject.Find("Crosshair");
		Crosshair.GetComponent<SpriteRenderer>().enabled = false;

		audioSource = GameObject.Find ("AudioManager/EffectsAudio").GetComponent<AudioSource> ();

		if(FirePoint == null){
			Debug.LogError ("No 'FirePoint' object found.");
		}
		if(weaponEffects == null) {
			Debug.LogError ("Script for weapon effects is missing!");
		}
		if(WeaponFireSoundEffect == null){
			Debug.LogError ("WeaponFireSoundEffect is missing!");
		}
	}

	void Update () {
		// Check which aiming mode is selected and do stuff accordingly
		if (Target == aimTowards.mouse) {

			// Player mouse position
			Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			Vector2 firePointPosition = new Vector2 (FirePoint.position.x, FirePoint.position.y);

			ShootTimer += Time.deltaTime;

			// Reloading
			if(Input.GetKeyDown(KeyCode.R) && ReloadComplete){
				ReloadComplete = false;
				StartCoroutine(Reload());
			}

			// Shooting
			if(weaponStats.WeaponType == WeaponTypes.full){
				if (Input.GetMouseButton (0) && ReloadComplete && CurrentAmmo > 0) {
					if(ShootTimer > weaponStats.FireRate){
						Shoot (mousePosition, firePointPosition);
						ShootTimer = 0;
					}
				}
			} else {
				if (Input.GetMouseButtonDown (0) && ReloadComplete && CurrentAmmo > 0) {
					if(ShootTimer > weaponStats.FireRate){
						Shoot (mousePosition, firePointPosition);
						ShootTimer = 0;
					}
				}
			}
		} else if (Target == aimTowards.player) {

			// booleans for checking if enemy or player is on camera
			bool playerVisible = player.transform.FindChild("Graphics").GetComponent<SpriteRenderer>().isVisible;
			bool enemyVisible = this.transform.GetComponent<SpriteRenderer>().isVisible;

			// Check if player is close enough to start aiming
			if (playerVisible && enemyVisible) { //playerDistance < 20.0f

				if (ShootTimer < RandomAimDelay) {
					ShootTimer += Time.deltaTime;
				} else if(ShootTimer > RandomAimDelay && ReloadComplete && CurrentAmmo > 0) { //if (ShootComplete > aimDelay)
					RandomAimDelay = Random.Range(EnemyAimDelay - 0.3f, EnemyAimDelay);
					ShootTimer = 0;
					Debug.Log ("ENEMY SHOOT");
					Vector3 playerPosition = new Vector3(player.transform.position.x + player.GetComponent<BoxCollider2D>().offset.x, 
					                                     player.transform.position.y + player.GetComponent<BoxCollider2D>().offset.y,
					                                     0f);
					//Debug.DrawRay (FirePoint.position, (playerPosition-FirePoint.position)*30, Color.white, 1);
					Shoot (playerPosition, FirePoint.position);

					// Enemy reload
					if(CurrentAmmo < 1){
						StartCoroutine(Reload());
					}
				}
			}
		}
	}

	IEnumerator Reload(){
		Debug.Log ("Reloading");
		yield return new WaitForSeconds(weaponStats.ReloadTime);
		CurrentAmmo = weaponStats.ClipSize;
		ReloadComplete = true;
		Debug.Log ("Done reloading");
	}

	void Aim(){		
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
	}


	void Shoot(Vector2 aimPosition, Vector2 firePointPosition){
		Vector3 hitDirection;
		Vector3 hitPosition; 
		Vector3 hitNormal;
		Collider2D collider;

		// reminder: http://answers.unity3d.com/questions/211910/getting-object-local-direction.html

		// raycast is used to see when player hits something
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, aimPosition-firePointPosition, HitRange, WhatToHit);

		// Firing sound effect
		audioSource.PlayOneShot (WeaponFireSoundEffect, 0.2f);

		if (hit.collider != null) {
			// Logic after we hit something
			Debug.Log ("Hit " + hit.collider.name + " and did " + weaponStats.Damage + " damage.");

			hitDirection = aimPosition-firePointPosition; // direction of the bullet
			hitPosition = hit.point; // point where bullet collided with something
			hitNormal = hit.normal; // normal vector of collided surface
			collider = hit.collider; // for different hit collision effects based on collision

			if(hit.collider.tag == "enemy") {
				hit.collider.GetComponent<Enemy>().DamageEnemy(weaponStats.Damage);
			} else if(hit.collider.tag == "hittable") {
				 // wall
			} else if(hit.collider.tag == "Player") {
				player.DamagePlayer(weaponStats.Damage);
			} else{
				//do something
			}

			weaponEffects.BulletTrail(hitPosition);
			weaponEffects.BulletHit(hitDirection ,hitPosition, hitNormal, collider);
		} else {
			hitPosition = (aimPosition-firePointPosition)*30; // Get the mouse cursor direction and add some distance
			hitPosition += FirePoint.position;
			hitNormal = new Vector3 (9999, 9999, 9999); // used for checking if bullet hit something in effect method... kinda bad solution but whatever
			collider = null;
			weaponEffects.BulletTrail(hitPosition);
		}

		// Remove bullet from the magazine
		CurrentAmmo -= 1;

		weaponEffects.MuzzleFlash ();


		//Debug.DrawRay (firePointPosition, (mousePosition-firePointPosition)*30, Color.white, 1);

	}
}