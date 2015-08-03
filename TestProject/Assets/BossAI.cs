using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

	private Boss boss;
	private Weapon Rifle;
	private Weapon Uzi;
	
	private float BossSpeed;
	private int MovingTowardsWaypoint;
	private int CurrentWaypoint;

	private bool ExecutionStopperWeapons = false;
	private bool ExecutionStopperGrenades = false;
	private bool Waiting = false;
	private float Timer;
	private float Timer2 = 0f;

	void Awake() {
		boss = GetComponent<Boss> ();
		Rifle = transform.GetChild (0).FindChild ("AWP").GetComponent<Weapon>();
		Uzi = transform.GetChild (0).FindChild ("Uzi").GetComponent<Weapon>();
	}

	void Update(){
		BossSpeed = boss.GetComponent<Rigidbody2D> ().velocity.magnitude;

		// Boss movement
		if (BossSpeed <= 0f) {
			// Get next random waypoint
			MovingTowardsWaypoint = Random.Range (0, 3);

			// Create a delay before boss starts moving
			if (Timer < 1.5f) {
				Timer += Time.deltaTime;
			} else {
				BossMovement(); // Set the boss in motion so code can continue to the else block
				Timer = 0;
				ExecutionStopperGrenades = false;
			}
		} else {
			BossMovement();
		}

		// Check if boss should use his rifle
		if (Vector3.Distance (boss.Waypoints [1].position, transform.position) < 1f && MovingTowardsWaypoint == 1) {
			BossShootRifle ();
		} else {
			// Dont't shoot for 0.5 seconds
			if(Timer2 <= 0.5f){
				Uzi.ExecuteAttack = false;
				Timer2 += Time.deltaTime;
			// Shoot for 0.5 seconds
			} else {
				BossShootUzi();
				Timer2 += Time.deltaTime;
			}

			// Reset Timer2
			if(Timer2 > 1f){
				Timer2 = 0f;
			}
		}

		// Check if boss should throw some grenades
		if (Vector3.Distance (boss.Waypoints [0].position, transform.position) < 0.2f && MovingTowardsWaypoint == 0 && ExecutionStopperGrenades == false) {
			StartCoroutine(boss.ThrowGrenades(4, -1));
			ExecutionStopperGrenades = true;
		} else if(Vector3.Distance (boss.Waypoints [2].position, transform.position) < 0.5f && MovingTowardsWaypoint == 2 && ExecutionStopperGrenades == false) {
			StartCoroutine(boss.ThrowGrenades(4, 1));
			ExecutionStopperGrenades = true;
		}

	}

	void BossMovement(){
		boss.MoveBoss (boss.Waypoints[MovingTowardsWaypoint]);
	}

	void BossShootRifle(){
		if(BossSpeed < 0.05f && ExecutionStopperWeapons == false){
			// Check if weapon switch is needed
			if(!Rifle.gameObject.activeInHierarchy){
				boss.SwitchWeapon ();
			}

			Debug.Log("Rifle Execute");
			Rifle.ExecuteAttack = true;
			ExecutionStopperWeapons = true;
		}
	}

	void BossShootUzi(){
		if (BossSpeed > 0.5f) {
			// Check if weapon switch is needed
			if(!Uzi.gameObject.activeInHierarchy){
				boss.SwitchWeapon ();
			}
			
			//Debug.Log ("Uzi Execute");
			Uzi.ExecuteAttack = true;
			ExecutionStopperWeapons = false;
		} else {
			//Debug.Log ("Uzi Stop");
			Uzi.ExecuteAttack = false;
		}
	}
	
}