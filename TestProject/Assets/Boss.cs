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

	public BossStats bossStats = new BossStats();

	public Transform[] Waypoints;
	public Transform Grenade;

	public Vector3[] localWaypoints;
	private Vector3[] globalWaypoints;

	private int fromWaypointIndex = 0;
	private float percentBetweenWaypoints;

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
		//this.transform.position = Waypoints [0].position;

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i =0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//MoveBoss (Waypoints[1].position);
		//MoveBoss (CalcMoveBoss());
		if (TestBoolean == false) {
			StartCoroutine(ThrowGrenades (10));
			TestBoolean = true;
		}

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

	public void DamageBoss(int damage){
		bossStats.CurrentHealth -= damage;
		
		if(bossStats.CurrentHealth <= 0){
			GameMaster.KillBoss(this);
		}
	}

	public void MoveBoss(Vector3 targetWaypoint){
		//BossRigidbody2D.velocity = velocity;
		BossRigidbody2D.velocity = (targetWaypoint - transform.position) * bossStats.MovementSpeed;
		//BossRigidbody2D.velocity = new Vector2(-1f * bossStats.MovementSpeed, BossRigidbody2D.velocity.y);
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

	public IEnumerator ThrowGrenades(int amount) {
		for(int i = 0; i < amount; i++) {
			// Create a small delay between grenades
			yield return new WaitForSeconds(0.2f);

			// Randomize direction of the grenades
			float randomX = Random.Range (-0.5f, 0.5f);
			float randomY = Random.Range (0.2f, 0.8f);
			float randomVelocity = Random.Range (5f, 10f);
			Vector3 direction = new Vector3 (randomX, randomY, 0f);

			// Instantiate the grenades
			Transform grenadeClone = Instantiate (Grenade, transform.position, transform.rotation) as Transform;
			grenadeClone.GetComponent<Rigidbody2D> ().velocity = direction * randomVelocity;
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