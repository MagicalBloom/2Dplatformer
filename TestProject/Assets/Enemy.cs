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

	private Rigidbody2D EnemyRigidbody2D;
	private Animator animator;

	public bool EnemyMoving = false;
	public float MovementDuration;
	private float MovementDirection;
	public float MovementSpeed;
	private float MovementTimer = 0;

	public enum EnemyType {still, runInFront, runInBack};
	public EnemyType enemyType;
	
	public EnemyStats enemyStats = new EnemyStats();
	
	void Awake (){
		// Set movement direction based on enemy type
		if (enemyType == EnemyType.runInBack) {
			MovementDirection = 1f;
		} else if (enemyType == EnemyType.runInFront) {
			MovementDirection = -1f; // REMEMBER VELOCITY AND ROTATION....OR NOT
		} else {
			MovementDirection = 0f;
		}

		enemyStats.Init ();
		EnemyRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	public void DamageEnemy(int damage){
		enemyStats.CurrentHealth -= damage;

		if(enemyStats.CurrentHealth <= 0){
			GameMaster.KillEnemy(this);
		}
	}

	void FixedUpdate(){
		if(enemyType == EnemyType.runInBack && EnemyMoving == true){
			MovementTimer += Time.fixedDeltaTime;
			if(MovementTimer < MovementDuration){
				MoveEnemy(MovementDirection * MovementSpeed); // FUCK
			} else {
				EnemyMoving = false;
				MovementTimer = 0;
			}
		}
	}

	public void MoveEnemy(float direction) {
		EnemyRigidbody2D.velocity = new Vector2(direction, EnemyRigidbody2D.velocity.y);
	}

}
