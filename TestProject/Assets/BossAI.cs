using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

	private Boss boss;
	private Weapon Rifle;
	private Weapon Uzi;
	
	private float BossSpeed;
	private int MovingTowardsWaypoint;

	private bool ExecutionStopper = false;
	private bool Waiting = false;
	private float Timer;

	void Awake() {
		boss = GetComponent<Boss> ();
		Rifle = transform.GetChild (0).FindChild ("AWP").GetComponent<Weapon>();
		Uzi = transform.GetChild (0).FindChild ("Uzi").GetComponent<Weapon>();
	}

	void Update(){
		BossSpeed = boss.GetComponent<Rigidbody2D> ().velocity.magnitude;

		if (BossSpeed <= 0f) {
			// Get next random waypoint
			MovingTowardsWaypoint = Random.Range (0, 3);

			// Create a delay before boss starts moving
			if (Timer < 1f) {
				Timer += Time.deltaTime;
			} else {
				BossMovement(); // Set the boss in motion so code can continue to the else block
				Timer = 0;
			}
		} else {
			BossMovement();
		}

		//Debug.Log (Vector3.Distance(boss.Waypoints[1].position, transform.position));

		if (Vector3.Distance (boss.Waypoints [1].position, transform.position) < 1f) {
			BossShootRifle();
		} else {
			BossShootUzi();
		}

		/*
		if (MovingTowardsWaypoint == 0) {
			BossShootUzi();
		} else if (Vector3.Distance(boss.Waypoints[1].position, transform.position) < 2f) {
			BossShootRifle();
		} else if (MovingTowardsWaypoint == 2) {
			BossShootUzi();
		} else {
			// something
		}
		*/
	}

	void BossMovement(){
		boss.MoveBoss (boss.Waypoints[MovingTowardsWaypoint]);
	}

	void BossShootRifle(){
		if(BossSpeed < 0.05f && ExecutionStopper == false){
			// Check if weapon switch is needed
			if(!Rifle.gameObject.activeInHierarchy){
				boss.SwitchWeapon ();
			}

			Debug.Log("Rifle Execute");
			Rifle.ExecuteAttack = true;
			ExecutionStopper = true;
		}
	}

	void BossShootUzi(){
		if (BossSpeed > 0.25f) {
			// Check if weapon switch is needed
			if(!Uzi.gameObject.activeInHierarchy){
				boss.SwitchWeapon ();
			}
			
			//Debug.Log ("Uzi Execute");
			Uzi.ExecuteAttack = true;
			ExecutionStopper = false;
		} else {
			//Debug.Log ("Uzi Stop");
			Uzi.ExecuteAttack = false;
		}
	}
	
}