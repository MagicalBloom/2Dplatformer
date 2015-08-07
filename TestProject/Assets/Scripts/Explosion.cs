using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	[System.Serializable]
	public class ExplosionStats{
		public float ExplosionSize; // Size of the explosion
		public int ExplosionDamage; // Damage og the explosion
		public float ExplosionDelay; // Delay before the explosive explode after thrown
		public float ExplosionSpeed; // How fast the explosion radius travels from 0 to ExplosionSize
		public float TroughWallsTime; // Time before explosive's collider is activated
	}

	public ExplosionStats explosionStats = new ExplosionStats();
	public GameObject ExplosionEffect;
	public AudioClip ExplosionSoundEffect;
	
	private AudioSource audioSource;
	private Player player;
	private CircleCollider2D ExplosionRadius;
	private CircleCollider2D ParentCollider;
	private float CurrentExplosionRadius;
	private bool Explode = false;
	private bool Exploded = false;
	private float ColliderTimer;

	void Awake () {
		ExplosionRadius = GetComponent<CircleCollider2D> ();

		// Disable collider so it can pass trough the floor when boss trows them. (This isn't good if we want to use this class later for other explosives :/)
		ParentCollider = transform.parent.GetComponent<CircleCollider2D> ();
		ParentCollider.enabled = false;

		player = GameObject.Find ("Player").GetComponent<Player> ();
		audioSource = GameObject.Find ("AudioManager/EffectsAudio").GetComponent<AudioSource> ();
	}

	void Update () {
		explosionStats.ExplosionDelay -= Time.deltaTime;
		ColliderTimer += Time.deltaTime;

		// Enable the collider after defined amount of time
		if(ColliderTimer > explosionStats.TroughWallsTime && Explode == false){
			ParentCollider.enabled = true;
		}

		// Set explode to true and with that, trigger the explosion
		if(explosionStats.ExplosionDelay < 0){
			Explode = true;
		}
	}

	void FixedUpdate(){
		
		if(Explode){
			// Check if the radius is stioll within defined explosion size
			if(CurrentExplosionRadius < explosionStats.ExplosionSize){

				CurrentExplosionRadius += explosionStats.ExplosionSpeed; // Set current radius
			}
			// Explosion radius is big enough and effect hasn't happened yet
			else if(CurrentExplosionRadius >= explosionStats.ExplosionSize && Exploded == false) {

				audioSource.PlayOneShot (ExplosionSoundEffect, 0.3f); // Play the sound effect
				GameObject clone = Instantiate(ExplosionEffect, transform.position, transform.rotation) as GameObject; // Instantiate the effect
				clone.transform.SetParent(transform); // Set effect's parent to the explosive so it'll be destroyed with it
				Destroy(this.transform.parent.gameObject, 0.2f); // Destroy the explosive object
				Exploded = true; // Effect fired
			}

			// Update the circle collider's size
			ExplosionRadius.radius = CurrentExplosionRadius;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		// Handle the damage given to the player
		if(Explode){
			if(collider.gameObject.GetComponent<Rigidbody2D>()){
				// Additional if statement for future uses
				if(collider.tag == "Player"){
					player.DamagePlayer(explosionStats.ExplosionDamage);
				}
			}
		}
	}
}
