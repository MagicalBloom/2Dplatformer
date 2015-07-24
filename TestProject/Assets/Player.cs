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
}