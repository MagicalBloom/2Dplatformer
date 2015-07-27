using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats{
		public int maxHealth = 100;
		public int regenAmount = 10;
		public float regenRate = 2;
		public float regenDelay = 5;
		private int curHealth;

		public int CurHealth{
			get { return curHealth; }
			set { curHealth = Mathf.Clamp(value, 0, maxHealth);}
		}

		public void Init(){
			curHealth = maxHealth;
		}

	}

	public PlayerStats playerStats = new PlayerStats();

	float regenTimerRate = 0;
	float regenTimerDelay = 0;

	void Start(){
		playerStats.Init ();
	}

	void Update(){
		// Player health regeneration
		regenTimerRate += Time.deltaTime;
		regenTimerDelay += Time.deltaTime;
		if(regenTimerRate > playerStats.regenRate && regenTimerDelay > playerStats.regenDelay){
			RegenPlayer();
			regenTimerRate = 0;
		}
	}

	public void RegenPlayer(){
		if (playerStats.CurHealth + playerStats.regenAmount <= playerStats.maxHealth) {
			playerStats.CurHealth += playerStats.regenAmount;
		} else {
			playerStats.CurHealth += playerStats.regenAmount - ((playerStats.CurHealth + playerStats.regenAmount) % playerStats.maxHealth);
		}
		GameMaster.UpdatePlayerHealthbar(playerStats.CurHealth);
	}

	public void DamagePlayer(int damage){
		playerStats.CurHealth -= damage;
		regenTimerDelay = 0; // Reset regenTimer so regen won't kick in if player takes damage

		GameMaster.UpdatePlayerHealthbar (playerStats.CurHealth);
		if(playerStats.CurHealth <= 0){
			GameMaster.KillPlayer(this);
		}
	}
}