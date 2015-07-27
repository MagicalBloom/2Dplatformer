using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats{
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

	public enum EnemyType {still, runInFront, runInBack};
	
	public EnemyStats enemyStats = new EnemyStats();
	
	void Start(){
		enemyStats.Init ();
	}
	
	public void DamageEnemy(int damage){
		enemyStats.CurHealth -= damage;

		if(enemyStats.CurHealth <= 0){
			GameMaster.KillEnemy(this);
		}
	}
}
