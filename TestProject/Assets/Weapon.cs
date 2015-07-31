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

	private Player player; //test
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

	private bool ClipIsEmpty = false;
	private bool Flag = true;

	void Start() {
		// Update player HUD
		CurrentAmmo = weaponStats.ClipSize;
		if(ObjectType == ObjectTypes.player){
			GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
		}
	}

	void Awake () {
		player = GameObject.Find("Player").GetComponent<Player>(); //test
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

			// Clip empty
			if(CurrentAmmo <= 0){
				ClipIsEmpty = true;
			}

			if(Input.GetMouseButton (0) && CurrentAmmo <= 0 && ClipIsEmpty && ReloadComplete){


			}

			// Shooting
			if(weaponStats.WeaponType == WeaponTypes.full){
				if (Input.GetMouseButton (0) && ReloadComplete && ShootTimer > weaponStats.FireRate) {
					if(CurrentAmmo > 0){
						Shoot (mousePosition, firePointPosition);
						GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
					} else {
						audioSource.PlayOneShot (WeaponClipEmptySoundEffect, 0.4f);
					}
					ShootTimer = 0;
				}
			} else {
				if (Input.GetMouseButtonDown (0) && ReloadComplete && ShootTimer > weaponStats.FireRate) {
					if(CurrentAmmo > 0){
						Shoot (mousePosition, firePointPosition);
						GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo);
					} else {
						audioSource.PlayOneShot (WeaponClipEmptySoundEffect, 0.4f);
					}
					ShootTimer = 0;
				}
			}
		// Logic for enemy shooting
		} else if (ObjectType == ObjectTypes.enemy) {

			// Check if player is close enough to start aiming
			if (playerVisible && enemyVisible) {

				// Initial delay before enemy starts to shoot
				bool startShooting = false;
				if(!startShooting){
					StartCoroutine(WaitSomeSeconds(EnemyAimDelay));
					startShooting = true;
				}

				// Enemy shooting logic
				if (ShootTimer < RandomizeFirerate && startShooting) {
					ShootTimer += Time.deltaTime;
				} else if(ShootTimer > RandomizeFirerate && ReloadComplete && CurrentAmmo > 0) {
					// Randomize enemy aim delay a bit
					RandomizeFirerate = Random.Range(weaponStats.FireRate - 0.3f, weaponStats.FireRate);

					// Reset the timer
					ShootTimer = 0;

					// Player position with collider offset
					Vector3 playerPosition = new Vector3(player.transform.position.x + player.GetComponent<BoxCollider2D>().offset.x, 
					                                     player.transform.position.y + player.GetComponent<BoxCollider2D>().offset.y,
					                                     0f);

					// Randomize enemy aiming a bit
					float minValX = playerPosition.x - 0.2f;
					float maxValX = playerPosition.x + 0.2f;
					float minValY = playerPosition.y - 0.2f;
					float maxValY = playerPosition.y + 0.2f;
					
					playerPosition.x = Random.Range(minValX,maxValX);
					playerPosition.y = Random.Range(minValY,maxValY);

					// Call the shoot method
					Shoot (playerPosition, FirePoint.position);

					// Enemy reload
					if(CurrentAmmo < 1){
						StartCoroutine(Reload());
					}
				}
			}
		} else if (ObjectType == ObjectTypes.boss) {
			if (playerVisible && enemyVisible) {
				// Player position with collider offset

				Vector3 playerPosition = new Vector3(player.transform.position.x + player.GetComponent<BoxCollider2D>().offset.x, 
				                                     player.transform.position.y + player.GetComponent<BoxCollider2D>().offset.y,
				                                     0f);
				if(Flag){
					StartCoroutine(Aim(playerPosition, FirePoint.position));
				}
			}
		}
	}

	// Just for creating delays
	IEnumerator WaitSomeSeconds(float seconds){
		yield return new WaitForSeconds(seconds);
	}

	IEnumerator Reload(){
		audioSource.PlayOneShot (WeaponReloadStartSoundEffect, 0.3f); // Clip out sound effect
		yield return new WaitForSeconds(weaponStats.ReloadTime); // Reload wait time
		audioSource.PlayOneShot (WeaponReloadEndSoundEffect, 0.3f); // Clip in sound effect
		CurrentAmmo = weaponStats.ClipSize; // Reset current ammo to max
		ReloadComplete = true;

		// Check if player is reloading
		if(ObjectType == ObjectTypes.player) {
			GUIManager.UpdatePlayerAmmo (weaponStats.ClipSize, CurrentAmmo); // Update player HUD
		}
	}

	IEnumerator Aim(Vector2 aimPosition, Vector2 firePointPosition){ //Vector2 aimPosition, Vector2 firePointPosition
		Debug.Log ("Start aiming");
		Flag = false;
		float time = 0;
		AimEffect(aimPosition, firePointPosition);

		while(time < weaponStats.ChargeTime){
			Arm.FreezeArmAndDirection = true;
			//Debug.DrawRay (firePointPosition, (aimPosition-firePointPosition), Color.white, 0.1f);
			time += Time.deltaTime;
			yield return null;
			//Debug.Log(time);
		}
		Arm.FreezeArmAndDirection = false;
		Debug.Log ("Stop aiming");
	}

	void AimEffect(Vector2 aimPosition, Vector2 firePointPosition){
		Debug.Log ("aimeffect");
		Transform line = Instantiate (AimTestPrefab, FirePoint.position, FirePoint.rotation) as Transform;
		LineRenderer linerenderer = line.GetComponent<LineRenderer> ();
		
		if(linerenderer != null){
			linerenderer.SetPosition(0, firePointPosition);
			linerenderer.SetPosition(1, (aimPosition - firePointPosition) * 3); // If aim is all over the place... fix this
		}
		Destroy (line.gameObject, weaponStats.ChargeTime);

	}

	void Shoot(Vector2 aimPosition, Vector2 firePointPosition){
		Vector3 hitDirection;
		Vector3 hitPosition; 
		Vector3 hitNormal;
		Collider2D collider;

		// reminder: http://answers.unity3d.com/questions/211910/getting-object-local-direction.html

		//HitRange = Camera.main.orthographicSize * Screen.width / Screen.height; half of camera width

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