using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats{
		public int MaxHealth = 100;
		public float MovementSpeed;

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

	[HideInInspector]
	public float MovementDuration;

	private float MovementDirection;
	private float MovementTimer = 0;

	public enum EnemyType {still, runInFront, runInBack};
	public EnemyType enemyType;
	
	public EnemyStats enemyStats = new EnemyStats();
	
	void Awake (){
		// Set movement direction based on enemy type
		if (enemyType == EnemyType.runInBack) {
			MovementDirection = 1f;
		} else if (enemyType == EnemyType.runInFront) {
			MovementDirection = -1f;
		} else {
			MovementDirection = 0f;
		}

		enemyStats.Init ();
		EnemyRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	public void DamageEnemy(int damage){
		// Take hit points from current health
		enemyStats.CurrentHealth -= damage;

		// If current health is 0 or less, kill the enemy
		if(enemyStats.CurrentHealth <= 0){
			GameMaster.KillEnemy(this);
		}
	}

	void FixedUpdate(){
		// Check if the enemy type is a moving one and that the EnemyMoving variable is set to true
		if((enemyType == EnemyType.runInBack || enemyType == EnemyType.runInFront) && EnemyMoving == true){

			MovementTimer += Time.fixedDeltaTime;

			// Move enemy based on timer
			if(MovementTimer < MovementDuration){
				MoveEnemy(MovementDirection * enemyStats.MovementSpeed);
			} else {
				EnemyMoving = false;
				MovementTimer = 0;
			}
		}

		animator.SetBool ("Moving", EnemyMoving);
	}

	public void MoveEnemy(float direction) {
		// Set velocity for the enemy
		EnemyRigidbody2D.velocity = new Vector2(direction, EnemyRigidbody2D.velocity.y);
	}

}
