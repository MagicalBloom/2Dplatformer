using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	[System.Serializable]
	public class BossStats{
		public int MaxHealth = 100;
		public float MovementSpeed;
		public float AimDelay;
		
		private int currentHealth;
		
		public int CurrentHealth{
			get { return currentHealth; }
			set { currentHealth = Mathf.Clamp(value, 0, MaxHealth);}
		}
		
		public void Init(){
			CurrentHealth = MaxHealth;
		}	
	}

	public BossStats bossStats = new BossStats();

	private Rigidbody2D EnemyRigidbody2D;
	private Animator animator;


	void Awake(){
		bossStats.Init ();
		EnemyRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
