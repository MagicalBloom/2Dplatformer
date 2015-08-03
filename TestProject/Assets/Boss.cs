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

	public static bool FreezeBoss = false;

	public BossStats bossStats = new BossStats();

	public Transform[] Waypoints; // right center left
	private Transform fromWaypoint;
	public Transform Grenade;

	private GameObject Rifle;
	private GameObject Uzi;

	public Vector3[] localWaypoints;
	private Vector3[] globalWaypoints;

	private int fromWaypointIndex = 0;
	private float percentBetweenWaypoints;

	private float CurrentSpeed = 0f;
	private float MovementTimer = 0f;
	private float MovementDuration = 4f;
	private bool EnemyMoving = true;

	private Rigidbody2D BossRigidbody2D;
	private Animator BossAnimator;

	private bool TestBoolean = false;


	void Awake(){
		bossStats.Init ();
		BossRigidbody2D = GetComponent<Rigidbody2D>();
		BossAnimator = GetComponent<Animator>();

		Rifle = transform.GetChild (0).FindChild ("AWP").gameObject;
		Uzi = transform.GetChild (0).FindChild ("Uzi").gameObject;

		fromWaypoint = Waypoints[0];
		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i =0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//MoveBoss (Waypoints[0].position);
		//MoveBoss (CalcMoveBoss());
		/*
		if (TestBoolean == false) {
			StartCoroutine(ThrowGrenades (4, -1));
			TestBoolean = true;
		}
		*/

		//Problems with stopping movement and getting the boss to stop at right position
		/*
		MovementTimer += Time.fixedDeltaTime;
		if(MovementTimer < MovementDuration && EnemyMoving == true){
			if(MovementTimer > MovementDuration - 0.5f){
				BossRigidbody2D.velocity = new Vector2(BossRigidbody2D.velocity.x - 1f * Time.deltaTime, 0f);
			} else {
				MoveBoss (Waypoints[1].position);
			}
		} else {
			//BossRigidbody2D.velocity = new Vector2(0.5f * bossStats.MovementSpeed, 0f); // Stop movement instantly
			BossRigidbody2D.velocity = new Vector2(0.5f * bossStats.MovementSpeed, 0f); // Stop movement gradually
			EnemyMoving = false;
			MovementTimer = 0;
		}
		*/

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

	public void MoveBoss(Transform targetWaypoint){
		//BossRigidbody2D.velocity = (targetWaypoint - transform.position) * bossStats.MovementSpeed;

		Vector3 direction = targetWaypoint.position - fromWaypoint.position;
		direction.Normalize();
		CurrentSpeed = Mathf.Lerp (CurrentSpeed, bossStats.MovementSpeed, Time.deltaTime * 4f); // Boss starts his movement gradually
		BossRigidbody2D.velocity = direction * CurrentSpeed; //direction * bossStats.MovementSpeed

		float distance = Vector3.Distance(targetWaypoint.position, fromWaypoint.position);
		percentBetweenWaypoints += Time.deltaTime * CurrentSpeed/distance; // bossStats.MovementSpeed/distance

		if(percentBetweenWaypoints >= 1){
			percentBetweenWaypoints = 0;
			CurrentSpeed = 0f;
			//Debug.Log("STOOOOOOOOOOOOOOOOOOOP");
			BossRigidbody2D.velocity = new Vector2(0, 0);
			fromWaypoint = targetWaypoint;
		}
	}

	Vector3 CalcMoveBoss() {
		// Lerp isn't working quite right with rigidbody
		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = fromWaypointIndex + 1;
		float distance = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);

		percentBetweenWaypoints += Time.deltaTime * bossStats.MovementSpeed/distance;
		//percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints);
		//float easedPercentBetweenWaypoints = Ease (percentBetweenWaypoints);

		Vector3 newPosition = Vector3.Lerp (globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], percentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex ++;

			if (fromWaypointIndex >= globalWaypoints.Length-1) {
				fromWaypointIndex = 0;
				System.Array.Reverse(globalWaypoints);
			}
		}

		return newPosition - transform.position;
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
		} else {
			Rifle.SetActive(true);
			Uzi.SetActive(false);
		}
	}

	void OnDrawGizmos() {
		// Visualize waypoints
		if (localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;
			
			for (int i =0; i < localWaypoints.Length; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}






















}