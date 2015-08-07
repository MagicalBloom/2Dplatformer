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
		public float ChargeTime;
	}

	public WeaponStats weaponStats = new WeaponStats();

	public enum ObjectTypes {player, enemy, boss};
	public enum WeaponTypes {full, semi, single};

	public float EnemyAimDelay;
	public LayerMask WhatToHit;
	public ObjectTypes ObjectType;

	public Transform AimTestPrefab;

	private WeaponEffects weaponEffects;

	private Player player;
	private Collider2D PlayerCollider;
	private Enemy enemy;
	private ArmRotation Arm;

	private int CurrentAmmo;
	private float RandomizeFirerate;
	private float HitRange = 45;
	private float ShootTimer;
	private float ReloadTimer;
	private bool ReloadComplete = true;
	private Transform FirePoint;

	private AudioSource audioSource;
	public AudioClip WeaponFireSoundEffect;
	public AudioClip WeaponReloadStartSoundEffect;
	public AudioClip WeaponReloadEndSoundEffect;
	public AudioClip WeaponClipEmptySoundEffect;

	public bool ExecuteAttack = false;

	void Start() {
		// Update player HUD
		CurrentAmmo = weaponStats.ClipSize;
		if(ObjectType == ObjectTypes.player){
			GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
		}
	}

	void Awake () {
		// Get all required components and objects
		player = GameObject.Find("Player").GetComponent<Player>();
		PlayerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
		FirePoint = transform.GetChild(0);
		weaponEffects = GetComponent<WeaponEffects> ();
		RandomizeFirerate = EnemyAimDelay;
		Arm = this.transform.parent.GetComponent<ArmRotation> ();

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

		// When player dies
		if (player == null)
			return;

		// booleans for checking if enemy or player is on camera
		bool playerVisible = player.transform.FindChild("Graphics").GetComponent<SpriteRenderer>().isVisible;
		bool enemyVisible = this.transform.GetComponent<SpriteRenderer>().isVisible;

		// Check which object type is selected and do stuff accordingly
		// Logic for player shooting
		if (ObjectType == ObjectTypes.player) {

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
			// Check if current weapon is automatic or something else
			if(weaponStats.WeaponType == WeaponTypes.full){

				// Check if mouse button is pressed, player isn't reloading or timer is less than firerate
				if (Input.GetMouseButton (0) && ReloadComplete && ShootTimer > weaponStats.FireRate) {

					// Check if there is still bullets left in the magazine
					if(CurrentAmmo > 0){
						Shoot (mousePosition, firePointPosition);
						GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
					}
					// No bullets left
					else {
						audioSource.PlayOneShot (WeaponClipEmptySoundEffect, 0.4f); // Empty magazine sound effect
					}

					ShootTimer = 0; // Reset the timer
				}
			}
			else {

				// Check if mouse button is pressed, player isn't reloading or timer is less than firerate
				if (Input.GetMouseButtonDown (0) && ReloadComplete && ShootTimer > weaponStats.FireRate) {

					// Check if there is still bullets left in the magazine
					if(CurrentAmmo > 0){
						Shoot (mousePosition, firePointPosition);
						GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
					}
					// No bullets left
					else {
						audioSource.PlayOneShot (WeaponClipEmptySoundEffect, 0.4f); // Empty magazine sound effect
					}

					ShootTimer = 0; // Reset the timer
				}
			}
		} // Logic for enemy shooting
		else if (ObjectType == ObjectTypes.enemy) {

			// Check if player is close enough to start aiming
			if (playerVisible && enemyVisible) {

				// Initial delay before enemy starts to shoot
				bool startShooting = false;
				if(!startShooting){
					GameMaster.WaitSomeSeconds(EnemyAimDelay);
					startShooting = true;
				}

				// Enemy shooting logic
				if (ShootTimer < RandomizeFirerate && startShooting) {
					ShootTimer += Time.deltaTime;
				}
				else if(ShootTimer > RandomizeFirerate && ReloadComplete && CurrentAmmo > 0) {
					// Randomize enemy aim delay a bit
					RandomizeFirerate = Random.Range(weaponStats.FireRate - 0.3f, weaponStats.FireRate);

					// Reset the timer
					ShootTimer = 0;

					// Call the Shoot method
					Shoot (playerPositionRandomized(0.2f), FirePoint.position);

					// Enemy reload
					if(CurrentAmmo < 1){
						StartCoroutine(Reload());
					}
				}
			}
		} // Logic for boss shooting
		else if (ObjectType == ObjectTypes.boss) {

			if (playerVisible && enemyVisible) {
				// Player position with collider offset
				Vector3 playerPosition = PlayerCollider.transform.TransformPoint(new Vector3(PlayerCollider.offset.x, PlayerCollider.offset.y, 0));

				if(weaponStats.WeaponType == WeaponTypes.single){ // Boss is using single shot rifle
					if(ExecuteAttack){ // Set the flag based on boss behaviour or something
						StartCoroutine(Aim(playerPosition, FirePoint.position));
					}
				} else { // Boss is using uzi
					if(ExecuteAttack){
						if(ShootTimer < weaponStats.FireRate){
							ShootTimer += Time.deltaTime;
						}
						else {
							// Reset the timer
							ShootTimer = 0;

							// Call the Shoot method
							Shoot (playerPositionRandomized(3f), FirePoint.position);
						}
					}
				}
			}
		}
	}

	// Method for randomizing enemy aiming
	Vector2 playerPositionRandomized(float randomness) {
		// Player position with collider offset
		Vector2 playerPosition = PlayerCollider.transform.TransformPoint(new Vector3(PlayerCollider.offset.x, PlayerCollider.offset.y));
		
		// Randomize enemy aiming a bit
		float minValX = playerPosition.x - randomness;
		float maxValX = playerPosition.x + randomness;
		float minValY = playerPosition.y - randomness;
		float maxValY = playerPosition.y + randomness;
		
		playerPosition.x = Random.Range(minValX,maxValX);
		playerPosition.y = Random.Range(minValY,maxValY);

		return playerPosition;
	}

	IEnumerator Reload(){
		audioSource.PlayOneShot (WeaponReloadStartSoundEffect, 0.4f); // Clip out sound effect
		yield return new WaitForSeconds(weaponStats.ReloadTime); // Reload wait time
		audioSource.PlayOneShot (WeaponReloadEndSoundEffect, 0.4f); // Clip in sound effect
		CurrentAmmo = weaponStats.ClipSize; // Reset current ammo to max
		ReloadComplete = true;

		// Check if player is reloading
		if(ObjectType == ObjectTypes.player) {
			GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo); // Update player HUD
		}
	}

	IEnumerator Aim(Vector2 aimPosition, Vector2 firePointPosition){
		Debug.Log ("Start aiming");
		Boss.FreezeBoss = true; // Stop boss movement
		ExecuteAttack = false;
		float time = 0;

		AimEffect(aimPosition, firePointPosition);

		while(time < weaponStats.ChargeTime){
			Arm.FreezeArmAndDirection = true; // Stop boss' arm from moving
			time += Time.deltaTime;
			yield return null;
		}

		Shoot(aimPosition, firePointPosition); // Shoot

		// Release
		Arm.FreezeArmAndDirection = false;
		Boss.FreezeBoss = false;
	}

	void AimEffect(Vector2 aimPosition, Vector2 firePointPosition){
		Vector2 hitPosition; 
		hitPosition = (aimPosition - firePointPosition) * 3;
		hitPosition += firePointPosition;

		Transform line = Instantiate (AimTestPrefab, FirePoint.position, FirePoint.rotation) as Transform;
		LineRenderer linerenderer = line.GetComponent<LineRenderer> ();
		
		if(linerenderer != null){
			linerenderer.SetPosition(0, firePointPosition);
			linerenderer.SetPosition(1, hitPosition); // If aim is all over the place... fix this (aimPosition - firePointPosition) * 3
		}

		Destroy (line.gameObject, weaponStats.ChargeTime);
	}

	void Shoot(Vector2 aimPosition, Vector2 firePointPosition){
		Vector3 hitDirection;
		Vector3 hitPosition; 
		Vector3 hitNormal;
		Collider2D collider;

		// reminder: http://answers.unity3d.com/questions/211910/getting-object-local-direction.html
		Vector2 cameraCornerPosition = Camera.main.ViewportToWorldPoint (new Vector3(1f, 1f, -10f));
		Vector2 playerPostition = new Vector2 (player.transform.position.x, player.transform.position.y);
		HitRange = Vector2.Distance(playerPostition, cameraCornerPosition); //half of camera width + distance between camera and player


		// raycast is used to see when player hits something
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, aimPosition-firePointPosition, HitRange, WhatToHit);

		// Firing sound effect
		audioSource.PlayOneShot (WeaponFireSoundEffect, 0.2f);

		// Check if we hit something
		if (hit.collider != null) {
			// Logic after we hit something
			//Debug.Log ("Hit " + hit.collider.name + " and did " + weaponStats.Damage + " damage.");

			hitDirection = aimPosition-firePointPosition; // direction of the bullet
			hitPosition = hit.point; // point where bullet collided with something
			hitNormal = hit.normal; // normal vector of collided surface
			collider = hit.collider; // for different hit collision effects based on collision

			if(hit.collider.tag == "enemy") {
				hit.collider.GetComponent<Enemy>().DamageEnemy(weaponStats.Damage);
			}
			else if(hit.collider.tag == "boss") {
				hit.collider.GetComponent<Boss>().DamageBoss(weaponStats.Damage);
			}
			else if(hit.collider.tag == "hittable") {
				 // If we make covers destructable put the damage for those here
			}
			else if(hit.collider.tag == "Player") {
				player.DamagePlayer(weaponStats.Damage);
			}
			else{
				//do something
			}

			weaponEffects.BulletTrail(hitPosition);
			weaponEffects.BulletHit(hitDirection ,hitPosition, hitNormal, collider);
		}
		// In case we didn't hit anything
		else {
			hitPosition = (aimPosition-firePointPosition)*30; // Get the mouse cursor direction and add some distance
			hitPosition += FirePoint.position;
			hitNormal = new Vector3 (9999, 9999, 9999); // used for checking if bullet hit something in effect method... kinda bad solution but whatever
			collider = null;

			weaponEffects.BulletTrail(hitPosition);
		}

		// Remove bullet from the magazine
		CurrentAmmo -= 1;

		// Effect for muzzle flash
		weaponEffects.MuzzleFlash ();						
	}
}