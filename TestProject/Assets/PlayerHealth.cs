using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int maxHitPoints = 100;
	public int currentHitPoints;

	bool playerDead;
	bool playerDamaged;
	
	void Awake () {
		currentHitPoints = maxHitPoints; // Starting hp
	}
	
	// Update is called once per frame
	void Update () {
		if(playerDamaged){
			//healthbar effect
			Debug.Log("Current health: " + currentHitPoints);
		} else {
			//revert the effect back to normal
		}

		playerDamaged = false;
	}

	public void playerTakeDamage(int incomingDamage){
		playerDamaged = true;
		currentHitPoints -= incomingDamage;
		Debug.Log ("You took " + incomingDamage + " damage");

		if(currentHitPoints <= 0){
			playerDead = true;
			playerDied();
		}
	}

	void playerDied(){
		// animation and voice + whatnot
		Debug.Log ("You died :(");
	}
}