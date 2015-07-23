using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats{
		public int health = 100;
		//jump height
		//runspeed
		//something like that
	}

	public PlayerStats playerStats = new PlayerStats ();

	public void DamagePlayer(int damage){
		playerStats.health -= damage;

		if(playerStats.health <= 0){
			GameMaster.KillPlayer(this);
		}
	}
	/*
	public int maxHitPoints = 100;
	public int currentHitPoints;

	bool playerIsDead;
	bool playerTookDamage;
	
	void Awake () {
		currentHitPoints = maxHitPoints; // Starting hp
		playerIsDead = false;
	}

	void Update () {
		if(playerTookDamage){
			// healthbar effect
			Debug.Log("Current health: " + currentHitPoints);
		} else {
			// revert the effect back to normal
		}

		playerTookDamage = false;
	}

	public void playerTakeDamage(int incomingDamage){
		playerTookDamage = true;
		currentHitPoints -= incomingDamage;
		Debug.Log ("You took " + incomingDamage + " damage");

		if(currentHitPoints <= 0 && playerIsDead == false){
			playerDied();
		}
	}

	void playerDied(){
		// animation and voice + whatnot
		playerIsDead = true;
		Debug.Log ("You died :(");
		Application.LoadLevel(Application.loadedLevel); // Restart the level without any fancy shit
	}
	*/
}