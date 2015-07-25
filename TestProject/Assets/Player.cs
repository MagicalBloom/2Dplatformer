using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats{
		public int maxHealth = 100;
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
	//GameMaster gameMaster;

	void Start(){
		playerStats.Init ();
		//gameMaster = GameObject.Find("GM").GetComponent<GameMaster>();
	}

	public void DamagePlayer(int damage){
		playerStats.CurHealth -= damage;

		GameMaster.UpdatePlayerHealthbar (playerStats.CurHealth);
		if(playerStats.CurHealth <= 0){
			GameMaster.KillPlayer(this);
		}
	}
}