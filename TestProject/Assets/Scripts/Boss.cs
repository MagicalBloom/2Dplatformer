using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour {

	[System.Serializable]
	public class BossStats{
		public int MaxHealth = 100;
		public float MovementSpeed;
		public float AimDelay;
		
		private int currentHealth;
		
		public int CurrentHealth{
			get { return currentHealth; }
			set { currentHealth = Mathf.Clamp(value, 0, MaxHealth);}
		}
		
		public void Init(){
			CurrentHealth = MaxHealth;
		}	
	}

	public static bool FreezeBoss = false; // For stopping boss movement

	public BossStats bossStats = new BossStats(); // Instantiate bossStats so we can use them 

	// boss movement
	public Transform[] Waypoints; // bad programming but now you have to add waypoints in this order:   0.right 1.center 2.left
	private Transform fromWaypoint;
	private float percentBetweenWaypoints;
	private float CurrentSpeed = 0f;
	private bool BossIsMoving = false;
	private bool Flipped = false;

	// boss weaponry
	public Transform Grenade;
	private GameObject Rifle;
	private GameObject Uzi;

	// components
	private Rigidbody2D BossRigidbody2D;
	private Animator BossAnimator;
	private Transform BossGraphics;


	void Awake(){
		bossStats.Init ();
		BossRigidbody2D = GetComponent<Rigidbody2D>();
		BossAnimator = GetComponent<Animator>();
		BossGraphics = transform.FindChild ("Graphics").transform as Transform;

		Rifle = transform.GetChild (0).FindChild ("AWP").gameObject;
		Uzi = transform.GetChild (0).FindChild ("Uzi").gameObject;

		fromWaypoint = Waypoints[0];
	}

	void Update(){
		// Stop boss from moving if freeze is set to true
		if(FreezeBoss == true){
			StopBossFromMoving();
		}

		BossAnimator.SetBool ("Moving", BossIsMoving);
	}

	void StopBossFromMoving(){
		// Set zero velocity to stop movement
		BossRigidbody2D.velocity = new Vector2(0, 0);
		BossRigidbody2D.angularVelocity = 0;
	}

	public void DamageBoss(int damage){
		// Take hit points from current health
		bossStats.CurrentHealth -= damage;

		// If current health is 0 or less, kill the boss
		if(bossStats.CurrentHealth <= 0){
			GameMaster.KillBoss(this);
		}
	}

	public bool MoveBoss(Transform targetWaypoint){
		// Get the direction boss should be running
		Vector3 direction = targetWaypoint.position - fromWaypoint.position;

		// Flip based on next waypoint and current local scale
		if(targetWaypoint.position.x < transform.position.x && Flipped == false && BossGraphics.transform.localScale.x != -1f){
			BossGraphics.transform.localScale = new Vector3(BossGraphics.transform.localScale.x * -1, BossGraphics.transform.localScale.y, BossGraphics.transform.localScale.z);
			Flipped = true;
		} else if(targetWaypoint.position.x > transform.position.x && Flipped == false && BossGraphics.transform.localScale.x != 1f){
			BossGraphics.transform.localScale = new Vector3(BossGraphics.transform.localScale.x * -1, BossGraphics.transform.localScale.y, BossGraphics.transform.localScale.z);
			Flipped = true;
		}

		direction.Normalize();

		// Calculate speed and assign it to velocity
		CurrentSpeed = Mathf.Lerp (CurrentSpeed, bossStats.MovementSpeed, Time.deltaTime * 4f); // Boss starts his movement gradually
		BossRigidbody2D.velocity = direction * CurrentSpeed;

		// Get the distance between two waypoints and calculate how much of that distance is still left
		float distance = Vector3.Distance(targetWaypoint.position, fromWaypoint.position);
		percentBetweenWaypoints += Time.deltaTime * CurrentSpeed/distance;

		// Check if waypoint has been reached and return true if it is
		if (percentBetweenWaypoints >= 1 || float.IsInfinity (percentBetweenWaypoints)) {

			// Reset variables and set fromWaypoint to the current one
			percentBetweenWaypoints = 0;
			CurrentSpeed = 0f;
			BossRigidbody2D.velocity = new Vector2 (0, 0);
			fromWaypoint = targetWaypoint;

			BossIsMoving = false;
			Flipped = false;
			return true;
		}
		else {
			BossIsMoving = true;
			return false;
		}

	}

	// Throw grenades to left or right with random angle and force
	public IEnumerator ThrowGrenades(int amount, int throwDirection) {

		//Debug.Log ("Throw grenades!");

		for(int i = 0; i < amount; i++) {

			// Create a small delay between grenades
			yield return new WaitForSeconds(0.2f);

			// Randomize direction of the grenades
			float randomX = Random.Range (0.3f, 0.6f) * throwDirection;
			float randomY = Random.Range (0f, 0.15f);
			float randomVelocity = Random.Range (5f, 25f);
			Vector3 direction = new Vector3 (randomX, randomY, 0f);

			// Instantiate the grenades
			Transform grenadeClone = Instantiate (Grenade, transform.position, transform.rotation) as Transform;
			grenadeClone.GetComponent<Rigidbody2D> ().velocity = direction * randomVelocity;
		}
	}

	// Switch weapons between Uzi and Rifle
	public void SwitchWeapon(){
		if (Rifle.activeInHierarchy) {
			Rifle.SetActive(false);
			Uzi.SetActive(true);
		}
		else {
			Rifle.SetActive(true);
			Uzi.SetActive(false);
		}
	}
}