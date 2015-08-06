using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats{
		public int MaxLives = 3;
		public int MaxHealth = 100;
		public int RegenAmount = 10;
		public float RegenRate = 2;
		public float RegenDelay = 5;
		private int CurrentHealth;
		private int CurrentLives;

		public int CurHealth{
			get { return CurrentHealth; }
			set { CurrentHealth = Mathf.Clamp(value, 0, MaxHealth);}
		}

		public int CurLives{
			get { return CurrentLives; }
			set { CurrentLives = Mathf.Clamp(value, 0, MaxLives); }
		}

		public void Init(){
			// Player will get max health after death, scene change or start
			CurrentHealth = MaxHealth;

			// Some horrifying design right here... easy way out
			if (PlayerPrefs.HasKey ("PlayerLivesRemaining")) {
				if(PlayerPrefs.GetInt ("PlayerLivesRemaining") >= MaxLives){
					CurrentLives = MaxLives;
				} else if(PlayerPrefs.GetInt ("PlayerLivesRemaining") <= 0){
					//CurrentLives = MaxLives; // for now.. need to edit gameover screen
				}else {
					CurrentLives = PlayerPrefs.GetInt ("PlayerLivesRemaining");
				}
			} else {
				CurrentLives = MaxLives;
				PlayerPrefs.SetInt("PlayerLivesRemaining", MaxLives);
			}
		}
	}

	public PlayerStats playerStats = new PlayerStats();

	float RegenTimerRate = 0;
	float RegenTimerDelay = 0;

	void Start(){
		playerStats.Init ();
		GUIManager.UpdatePlayerLives(playerStats.CurLives);
	}

	void Update(){
		// Player health regeneration
		RegenTimerRate += Time.deltaTime;
		RegenTimerDelay += Time.deltaTime;
		if(RegenTimerRate > playerStats.RegenRate && RegenTimerDelay > playerStats.RegenDelay){
			RegenPlayer();
			RegenTimerRate = 0;
		}
	}

	public void RegenPlayer(){
		if (playerStats.CurHealth + playerStats.RegenAmount <= playerStats.MaxHealth) {
			playerStats.CurHealth += playerStats.RegenAmount;
		} else {
			playerStats.CurHealth += playerStats.RegenAmount - ((playerStats.CurHealth + playerStats.RegenAmount) % playerStats.MaxHealth);
		}
		GUIManager.UpdatePlayerHealthbar(playerStats.CurHealth);
	}

	public void DamagePlayer(int damage){
		playerStats.CurHealth -= damage;
		RegenTimerDelay = 0; // Reset regenTimer so regen won't kick in if player takes damage

		GUIManager.UpdatePlayerHealthbar (playerStats.CurHealth);
		if(playerStats.CurHealth <= 0){
			playerStats.CurLives -= 1;
			PlayerPrefs.SetInt("PlayerLivesRemaining", playerStats.CurLives);
			GUIManager.UpdatePlayerLives(playerStats.CurLives);
			GameMaster.KillPlayer(this);
		}
	}
}