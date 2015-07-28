using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats{
		public int MaxHealth = 100;
		private int currentHealth;
		
		public int CurrentHealth{
			get { return currentHealth; }
			set { currentHealth = Mathf.Clamp(value, 0, MaxHealth);}
		}
		
		public void Init(){
			CurrentHealth = MaxHealth;
		}	
	}

	public enum EnemyType {still, runInFront, runInBack};
	public EnemyType enemyType;
	
	public EnemyStats enemyStats = new EnemyStats();
	
	void Start(){
		enemyStats.Init ();
	}
	
	public void DamageEnemy(int damage){
		enemyStats.CurrentHealth -= damage;

		if(enemyStats.CurrentHealth <= 0){
			GameMaster.KillEnemy(this);
		}
	}

	void Update(){
		if(enemyType == EnemyType.runInFront){
			//Vector3 temp = new Vector3(-0.2f, 0f, 0f);
			//this.transform.position += temp;
		}
	}

}
