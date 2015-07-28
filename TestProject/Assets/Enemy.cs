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

	private Rigidbody2D rigidbody2D;
	private Animator animator;

	public enum EnemyType {still, runInFront, runInBack};
	public EnemyType enemyType;
	
	public EnemyStats enemyStats = new EnemyStats();
	
	void Awake (){
		enemyStats.Init ();
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	public void DamageEnemy(int damage){
		enemyStats.CurrentHealth -= damage;

		if(enemyStats.CurrentHealth <= 0){
			GameMaster.KillEnemy(this);
		}
	}

	void FixedUpdate(){
		if(enemyType == EnemyType.runInBack){
			//MoveEnemy(new Vector2(10f, rigidbody2D.position.y));
		}
	}

	public void MoveEnemy(Vector2 movement) {
		rigidbody2D.MovePosition(rigidbody2D.position + movement);
	}

}
