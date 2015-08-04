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

	// boss weaponry
	public Transform Grenade;
	private GameObject Rifle;
	private GameObject Uzi;

	// components
	private Rigidbody2D BossRigidbody2D;
	private Animator BossAnimator;


	void Awake(){
		bossStats.Init ();
		BossRigidbody2D = GetComponent<Rigidbody2D>();
		BossAnimator = GetComponent<Animator>();

		Rifle = transform.GetChild (0).FindChild ("AWP").gameObject;
		Uzi = transform.GetChild (0).FindChild ("Uzi").gameObject;

		fromWaypoint = Waypoints[0];
	}

	void Update(){
		if(FreezeBoss == true){
			StopBossFromMoving();
		}
	}

	void StopBossFromMoving(){
		BossRigidbody2D.velocity = new Vector2(0, 0);
		BossRigidbody2D.angularVelocity = 0;
	}

	public void DamageBoss(int damage){
		bossStats.CurrentHealth -= damage;
		
		if(bossStats.CurrentHealth <= 0){
			GameMaster.KillBoss(this);
		}
	}

	public bool MoveBoss(Transform targetWaypoint){
		Vector3 direction = targetWaypoint.position - fromWaypoint.position;
		direction.Normalize();
		CurrentSpeed = Mathf.Lerp (CurrentSpeed, bossStats.MovementSpeed, Time.deltaTime * 4f); // Boss starts his movement gradually
		BossRigidbody2D.velocity = direction * CurrentSpeed; //direction * bossStats.MovementSpeed

		float distance = Vector3.Distance(targetWaypoint.position, fromWaypoint.position);
		percentBetweenWaypoints += Time.deltaTime * CurrentSpeed/distance; // bossStats.MovementSpeed/distance

		// Check if waypoint has been reached
		if (percentBetweenWaypoints >= 1 || float.IsInfinity (percentBetweenWaypoints)) {
			percentBetweenWaypoints = 0;
			CurrentSpeed = 0f;
			BossRigidbody2D.velocity = new Vector2 (0, 0);
			fromWaypoint = targetWaypoint;

			return true;
		}
		else {
			return false;
		}

	}

	// Throw grenades to left or right with random angle and force
	public IEnumerator ThrowGrenades(int amount, int throwDirection) {
		Debug.Log ("Throw grenades!");
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